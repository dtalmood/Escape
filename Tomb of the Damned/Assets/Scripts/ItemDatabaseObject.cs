using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    // array of all items that exist in game
    public ItemObject[] Items;

    // second Dictionary needed instead of double for loops (better performance)
    // key is int, return ItemObject as value
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            // item id set during serialization
            Items[i].Id = i;
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        // populates dictionary with ref. values from Items array
        GetItem = new Dictionary<int, ItemObject>();
    }
}
