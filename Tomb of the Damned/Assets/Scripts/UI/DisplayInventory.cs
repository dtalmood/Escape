using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using JetBrains.Annotations;
using UnityEngine.Events;

public class DisplayInventory : MonoBehaviour
{
    public List<InventoryUISlot> inventorySlots;
    public InventoryObject playerInventory;



    public void Start()
    {
        if(playerInventory.onAddItem == null)
        {
            playerInventory.onAddItem = new UnityEvent();
        }
        playerInventory.onAddItem.AddListener(Setup);

        Setup();
    }

    public void Setup()
    {
        ClearSlots();
        List<InventorySlot> playerItems = playerInventory.Container.Items;
        int count = Mathf.Min(playerItems.Count, inventorySlots.Count);
        for (int i = 0; i < count; i++)
        {
            InventoryUISlot currentSlot = inventorySlots[i];
            currentSlot.SetItem(playerItems[i]);
        }
    }

    public void ClearSlots()
    {
        for(int i = 0; i < inventorySlots.Count; i++)
        {
            inventorySlots[i].ClearItem();
        }
    }

}