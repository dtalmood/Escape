using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    public SceneReference gameWonScene;

    public enum CarPart {Piston, Tube, Radiator,Wheel, NotCarPart}
    public CarPart currentPromptPart;

    public CarDoor carDoor;
    public Canvas partApplicationCanvas;
    public TMP_Text promptText;
    public InventoryObject playerInventory;

    public List<Item> installedItems;
    public List<ItemObject> requiredItems;
    

    public Transform wheelTransform;
    public Transform playerTransform;

    /// <summary>
    /// Returns true if the player is in range of the hood and the hood is open.
    /// </summary>
    public bool CanApplyEnginePart {
        get {
            return (
                carDoor.GetHoodStatus() && 
                carDoor.GetInHoodRange() &&
                HasEnginePart()
            );
        } 
    }

    public bool CanApplyWheel {
        get {
            //distance on x and z axis. Ignore vertical distance
            float distanceToWheel = Vector2.Distance(
                new Vector2(wheelTransform.position.x, wheelTransform.transform.position.z),
                new Vector2(playerTransform.position.x, playerTransform.transform.position.z)
            );
            bool distanceWithinTolerance = distanceToWheel < 1.5f;
            bool hasPart = HasWheelPart();

            //Debug.Log($"distanceWithinTolerance: {distanceWithinTolerance}, hasPart {hasPart}");

            return distanceWithinTolerance && hasPart; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CanApplyEnginePart)
        {
            CarUI(true);
        }
        else if(CanApplyWheel)
        { 
            CarUI(false);
        }
        else
        {
            if (partApplicationCanvas.gameObject.activeSelf)
            {
                partApplicationCanvas.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Set up the car UI. If hood it true, sets it up for the engine parts. Otherwise sets it up at the wheel.
    /// </summary>
    public void CarUI(bool hood)
    {   
        //Set the canvas active if it is not active
        if(!partApplicationCanvas.gameObject.activeSelf)
        {
            partApplicationCanvas.gameObject.SetActive(true);
        }
        CarPart part = PartInInventory(playerInventory, hood, out Item it);
        //Debug.Log($"part in inventory: {part}");
        UpdatePromptPosition(hood);
        UpdatePromptText(part, hood);
               
        if(Input.GetKeyDown(KeyCode.E) && part != CarPart.NotCarPart)
        {
            InstallPart(playerInventory, it);
        }
    }

    /// <summary>
    /// Update the position and rotation of the canvas with the prompt.
    /// </summary>
    private void UpdatePromptPosition(bool hood)
    {
        if(hood)
        {
            partApplicationCanvas.transform.localPosition =  new Vector3(-0.115f, 1, 2.65f);
            partApplicationCanvas.transform.localRotation = Quaternion.Euler( new Vector3(0, 180, 0));
        }
        else
        {
            partApplicationCanvas.transform.localPosition = new Vector3(-1.3f, 0.65f, -1.5f);
            partApplicationCanvas.transform.localRotation = Quaternion.Euler( new Vector3(0, 90f, 0));
        }

    }


    private void UpdatePromptText(CarPart part, bool hood)
    {
        //If we are already prompting to install the car part, we  don't need to update the text
        if(part == currentPromptPart)
        {
            return;
        }

        if(part == CarPart.NotCarPart)
        {
            promptText.text = "";
            //Store the part type we are currently prompting the user to install
            currentPromptPart = part;
            return;
        }

        //If we are standing at the hood, we shouldn't add the wheel.
        //If not at the hood (this should be the case at the back of the car), we should not 
        //be able to add engine parts, only the wheel
        if ((hood && part == CarPart.Wheel) || (!hood && part != CarPart.Wheel))
        {
            return;
        }

        promptText.text = $"Install {part.ToString()}";
        //Store the part type we are currently prompting the user to install
        currentPromptPart = part;
    }

    private bool HasWheelPart()
    {
        CarPart p = PartInInventory(playerInventory, false, out Item it);
        return (p == CarPart.Wheel);
    }

    private bool HasEnginePart()
    {
        CarPart p = PartInInventory(playerInventory, true, out Item it);
        return !(p == CarPart.NotCarPart || p == CarPart.Wheel);
    }


    /// <summary>
    /// Gets the first car part in the inventory that we can apply to the car
    /// </summary>
    private CarPart PartInInventory(InventoryObject inventory, bool EnginePartsOnly, out Item item)
    {
        foreach(InventorySlot slot in inventory.Container.Items)
        {
            //Only get engine parts
            if(EnginePartsOnly)
            {
                //We are going to use the item ID of the various car parts to get the display name
                switch (slot.item.Id)
                {
                    case 0: item = slot.item; return CarPart.Tube;
                    case 2: item = slot.item; return CarPart.Radiator;
                    case 3: item = slot.item; return CarPart.Piston;
                }
            }
            //If not at the engine, we should be trying to apply a wheel
            else
            {
                if(slot.item.Id == 1)
                {
                    item = slot.item; return CarPart.Wheel;
                }
            }
        }

        //If none of the inventory items were the correct part, we return null and no car part
        item = null;
        return CarPart.NotCarPart;
    }

    /// <summary>
    /// Add the car part item to the car and remove it from the player inventory. End the game if we have all parts.
    /// </summary>
    public void InstallPart(InventoryObject inventory,  Item item)
    {
        //If the installed items list is null create a new list
        installedItems ??= new List<Item>();
        installedItems.Add(item);
        inventory.RemoveItem(item, 100);
        
        if(AllPartsInstalled())
        {
            WinGame();
        }
    }

    /// <summary>
    /// If 4 parts are installed, it means we have all of the parts
    /// </summary>
    private bool AllPartsInstalled()
    {
        return (installedItems.Count == 4);
    }

    private void WinGame()
    {
        Time.timeScale = 0;
        SceneManager.LoadSceneAsync(gameWonScene, LoadSceneMode.Additive);
        Debug.Log("You win!");
    }

}
