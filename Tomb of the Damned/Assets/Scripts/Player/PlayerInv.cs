using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]

public class PlayerInv : MonoBehaviour
{
    public InventoryObject inventory;

    public void OnApplicationQuit()
    {
        inventory.Container.Clear();
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
