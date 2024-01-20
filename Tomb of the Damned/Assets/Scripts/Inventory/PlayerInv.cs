using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]

public class PlayerInv : MonoBehaviour
{
    public InventoryObject inventory;

    private void Update()
    {
        // saves inv when T is pressed
        if (Input.GetKeyDown(KeyCode.T))
        {
            inventory.Save();
        }

        // loads inv when L is pressed
        if (Input.GetKeyDown(KeyCode.L))
        {
            inventory.Load();
        }
    }

    public void OnApplicationQuit()
    {
        inventory.Container.Items.Clear();
    }


    /*public void AddtoInventory()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray,out hitInfo))
            {
                if(hitInfo.collider.gameObject.tag == "pickup")
                {
                    inventory.AddItem(item.item, 1);
                    Destroy(other.gameObject);
                }
            }
        }
    }*/

}