using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUISlot : MonoBehaviour
{
    public Image backdrop;
    public Image itemImage;
    public TMP_Text itemCount;
    [Tooltip("The inventory slot that this UI element corresponds to.")]
    public InventorySlot slot;


    public void SetItem(InventorySlot itemSlot)
    {
        if(itemSlot != null)
        {
            this.itemImage.sprite = itemSlot.item.sprite;
            this.itemCount.text = $"{itemSlot.amount}";
        }
        this.itemImage.enabled = itemSlot != null;
        this.itemCount.enabled = itemSlot != null;
    }

    public void ClearItem()
    {
        this.slot = null;
        this.itemImage.sprite = null;
        this.itemImage.enabled = false;
        this.itemCount.enabled = false;
    }
}
