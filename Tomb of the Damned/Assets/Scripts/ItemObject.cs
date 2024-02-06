using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    potions,
    equipment,
    Default,
    carPart,
    testing
}

public abstract class ItemObject : ScriptableObject
{
    public int Id;
    // holds display for item
    public Sprite uiDisplay;
    // store type of item
    public ItemType type;
    [TextArea(15, 20)]
    // description of item
    public string description;
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id;
    public Sprite sprite;
    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.Id;
        this.sprite = item.uiDisplay;
    }
}