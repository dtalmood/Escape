using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;

    /*private void OnEnable()
    {
#if UNITY_EDITOR
        // sets database to where the file is inside Unity
        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/DataB/Database.asset", typeof(ItemDatabaseObject));
#else
        database = Resources.Load<ItemDatabaseObject>("Database");
#endif
    }*/
    public void AddItem(Item _item, int _amount)
    {
        // check if item is in inv
        for (int i = 0; i < Container.Items.Count; i++)
        {
            // if it's in the player inv
            if (Container.Items[i].item == _item)
            {
                // add to it
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }
        // otherwise add new slot
        Container.Items.Add(new InventorySlot(_item.Id, _item, _amount));
    }

    [ContextMenu("Save")]
    public void Save()
    {
        /*// serialize scritable object to a string
        string saveData = JsonUtility.ToJson(this, true);
        // binary formater to create a file and write the string into the file (save)
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();*/

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load() 
    { 
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            /*BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();*/

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Container = (Inventory)formatter.Deserialize(stream);
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container = new Inventory(); 
    }

    /*public void OnAfterDeserialize()
    {
        for (int i = 0; i < Container.Items.Count; i++)
        {
            // adds item from the ID
            Container.Items[i].item = database.GetItem[Container.Items[i].ID];
        }
    }*/
}
[System.Serializable]
public class Inventory 
{
    public List<InventorySlot> Items = new List<InventorySlot>();

}
[System.Serializable]
public class InventorySlot
{
    // item and amount of it
    public int ID;
    public Item item;
    public int amount;

    // constructor
    public InventorySlot(int _id, Item _item, int _amount)
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