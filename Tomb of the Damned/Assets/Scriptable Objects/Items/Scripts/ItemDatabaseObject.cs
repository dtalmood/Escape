using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    // array of all items that exist in game
    public ItemObject[] Items;

    // put in an item object into Dictionary and will return id of item
    public Dictionary<ItemObject, int> GetId = new Dictionary<ItemObject, int>();

    // second Dictionary needed instead of double for loops (better performance)
    // key is int, return ItemObject as value
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    public void OnAfterDeserialize()
    {
        // clears dictionary
        GetId = new Dictionary<ItemObject, int>();
        // populates dictionary with ref. values from Items array
        GetItem = new Dictionary<int, ItemObject>();
        for (int i = 0; i < Items.Length; i++)
        {
            GetId.Add(Items[i], i);
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        
    }
}
