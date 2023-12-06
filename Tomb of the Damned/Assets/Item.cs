using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemObject item;
    private bool playerInRange;
    private PlayerInv playerInventory = null;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInventory = other.gameObject.GetComponent<PlayerInv>();
            playerInRange = true;
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;

        }
    }

    public void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("item" + item);
            if (item)
            {
                Debug.Log("Found item");
                playerInventory.inventory.AddItem(item, 1);
                Destroy(this.gameObject);
            }
        }
    }
}