using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarController : MonoBehaviour
{
    public enum CarPart {Piston, Tube, Radiator,Wheel, NotCarPart}
    public CarPart currentPromptPart;

    public CarDoor carDoor;
    public Canvas partApplicationCanvas;
    public TMP_Text promptText;
    public InventoryObject playerInventory;

    public List<Item> installedItems;
    public List<Item> requiredItems;

    /// <summary>
    /// Returns true if the player is in range of the hood and the hood is open.
    /// </summary>
    public bool CanApplyEnginePart {
        get {
            return (
                carDoor.GetHoodStatus() && 
                carDoor.GetInHoodRange()
            );
        } 
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void HoodUI()
    {
        if (CanApplyEnginePart)
        {
            //Set the canvas active if it is not active
            if(!partApplicationCanvas.gameObject.activeSelf)
            {
                partApplicationCanvas.gameObject.SetActive(true);
            }
            CarPart part = PartInInventory(playerInventory, out Item it);
            UpdatePromptText(part, true);
            
            if(Input.GetKey(KeyCode.E) && part != CarPart.NotCarPart)
            {
                InstallPart(playerInventory, it);
            }
        }
        else
        {

        }
    }

    private void TryApplyCarPart()
    {

    }


    private void UpdatePromptText(CarPart part, bool hood)
    {
        //If we are already prompting to install the car part, we  don't need to update the text
        if(part == currentPromptPart || part == CarPart.NotCarPart)
        {
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

    private CarPart PartInInventory(InventoryObject inventory, out Item item)
    {
        foreach(InventorySlot slot in inventory.Container.Items)
        {
            //We are going to use the item ID of the various car parts to get the display name
            switch(slot.item.Id)
            {
                case 0: item = slot.item; return CarPart.Tube;
                case 2: item = slot.item; return CarPart.Radiator;
                case 3: item = slot.item; return CarPart.Piston;
                //Should fall to the wheel last
                case 1: item = slot.item; return CarPart.Wheel;
            }
        }

        item = null;
        return CarPart.NotCarPart;
    }

    public void InstallPart(InventoryObject inventory,  Item item)
    {
        installedItems ??= new List<Item>();
        installedItems.Add(item);

        inventory.RemoveItem(item, 1);
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

    }

}
