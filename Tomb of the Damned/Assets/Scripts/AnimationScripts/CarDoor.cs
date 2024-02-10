using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDoor : MonoBehaviour
{
    private bool playerInRange = false;
    private bool doorStatus = false; // False = Close, True = Open
    public Animator carAnimation;

    void Awake()
    {
        if (carAnimation == null)
        {
            Debug.LogWarning("Animator component not found on: " + gameObject.name);
        }
    }

    void Update()
    {
        // Check if the 'E' key is pressed and the player is in range
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            if(!doorStatus) // Door Is Closed to so open it 
                carAnimation.SetBool("Door", true);
            
            else
                carAnimation.SetBool("Door", false);
            // Log the interaction to the console
            Debug.Log("Door interaction triggered.");
            doorStatus = !doorStatus;
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Press 'E' to interact with the door.");
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player is no longer in range.");
        }
    }
}
