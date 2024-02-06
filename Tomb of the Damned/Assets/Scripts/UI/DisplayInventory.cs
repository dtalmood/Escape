using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using JetBrains.Annotations;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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

    // function used for creating new events
    /*public void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        // get event trigger from game object
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        // set event type
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }*/

}