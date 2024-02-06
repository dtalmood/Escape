using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InventoryUISlot : MonoBehaviour
{
    // connects to DisplayInventory script
    // used to call AddEvent function
    // public DisplayInventory displayInv;
    public MouseItem mouseItem;

    public Image backdrop;
    public Image itemImage;
    public TMP_Text itemCount;
    [Tooltip("The inventory slot that this UI element corresponds to.")]
    public InventorySlot slot;

    // function used for creating new events
    public void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        // get event trigger from game object
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        // set event type
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
    public void SetItem(InventorySlot itemSlot)
    {
        if(itemSlot != null)
        {
            this.itemImage.sprite = itemSlot.item.sprite;
            this.itemCount.text = $"{itemSlot.amount}";
        }
        this.itemImage.enabled = itemSlot != null;
        this.itemCount.enabled = itemSlot != null;

        AddEvent(this.itemImage.gameObject, EventTriggerType.PointerEnter, delegate { OnEnter(this.itemImage.gameObject); });
        AddEvent(this.itemImage.gameObject, EventTriggerType.PointerExit, delegate { OnExit(this.itemImage.gameObject); });
        AddEvent(this.itemImage.gameObject, EventTriggerType.BeginDrag, delegate { OnDragStart(this.itemImage.gameObject); });
        AddEvent(this.itemImage.gameObject, EventTriggerType.EndDrag, delegate { OnDragEnd(this.itemImage.gameObject); });
        AddEvent(this.itemImage.gameObject, EventTriggerType.Drag, delegate { OnDrag(this.itemImage.gameObject); });
    }

    public void OnEnter(GameObject obj)
    {

    }

    public void OnExit(GameObject obj)
    {

    }

    public void OnDragStart(GameObject obj)
    {
        // copy of item (visual representation of moving the item)
        var mouseObject = new GameObject();
        // add RectTrans (rt) to mouseObject
        var rt = mouseObject.AddComponent<RectTransform>();
        // sprite we are displaying is same size of sprite in inventory 
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);
        // check if there is an item in the item slot we want to drag
        // if (itemsDisplayed[obj].ID >= 0)
        if (this.itemImage.enabled)
    }

    public void OnDragEnd(GameObject obj)
    {

    }

    public void OnDrag(GameObject obj)
    {
        // update mouseObject position to be the same as our mouse current position


    }

    public class MouseItem 
    {
        // object we created
        public GameObject obj;
        // item represented of the object we click on
        public InventorySlot item;
        // store slot we are hovering over and object hovering over
        public InventoryObject hoverItem;
        public GameObject hoverObj;
    }

    public void ClearItem()
    {
        this.slot = null;
        this.itemImage.sprite = null;
        this.itemImage.enabled = false;
        this.itemCount.enabled = false;
    }
}
