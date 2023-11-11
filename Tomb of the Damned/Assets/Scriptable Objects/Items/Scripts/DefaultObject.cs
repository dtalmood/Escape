using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Default")]
public class DefaultObject : ItemObject
{
    public void Awake()
    {
        // item type is default (don't have to manually set item type)
        type = ItemType.Default;
    }
}
