using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    private ItemDatabaseObject database;
    public List<InventorySlot> Container = new List<InventorySlot>();

    private void OnEnable()
    {
#if UNITY_EDITOR
        // sets database to where the file is inside Unity
        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/DataB/Database.asset", typeof(ItemDatabaseObject));
#else
        database = Resources.Load<ItemDatabaseObject>("Database");
#endif
    }
    public void AddItem(ItemObject _item, int _amount)
    {
        // check if item is in inv
        for (int i = 0; i < Container.Count; i++)
        {
            // if it's in the player inv
            if (Container[i].item == _item)
            {
                // add to it
                Container[i].AddAmount(_amount);
                return;
            }
        }
        // otherwise add new slot
        Container.Add(new InventorySlot(database.GetId[_item], _item, _amount));
    }

    public void Save()
    {
        // serialize scritable object to a string
        string saveData = JsonUtility.ToJson(this, true);
        // binary formater to create a file and write the string into the file (save)
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();
    }

    public void Load() 
    { 
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Container.Count; i++)
        {
            // adds item from the ID
            Container[i].item = database.GetItem[Container[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {
        
    }
}

[System.Serializable]
public class InventorySlot
{
    // item and amount of it
    public int ID;
    public ItemObject item;
    public int amount;

    // constructor
    public InventorySlot(int _id, ItemObject _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        // add the value to the current amount
        amount += value;
    }

}