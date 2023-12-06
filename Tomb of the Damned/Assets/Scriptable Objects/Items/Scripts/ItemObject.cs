using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    potions,
    equipment,
    Default
}

public abstract class ItemObject : ScriptableObject
{
    // holds display for item
    public GameObject prefab;
    // store type of item
    public ItemType type;
    [TextArea(15, 20)]
    // description of item
    public string description;
}