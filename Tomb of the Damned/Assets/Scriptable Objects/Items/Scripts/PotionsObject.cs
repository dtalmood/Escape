using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion Object", menuName = "Inventory System/Items/Potions")]
public class PotionsObject : ItemObject
{
    public int restoreHPValue;
    public void Awake()
    {
        type = ItemType.potions;
    }
}
