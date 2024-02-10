using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDoor : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip doorOpen;
    public AudioClip doorClose;
    private bool playerInCarRange = false;
    private bool playerInHoodRange = false;
    private bool doorStatus = false; // False = Close, True = Open
    private bool hoodStatus = false;
    public Animator carAnimation;
    public Animator hoodAnimation;

    void Awake()
    {
        if (carAnimation == null)
        {
            Debug.LogWarning("Animator component not found on: " + gameObject.name);
        }

        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component not found on: " + gameObject.name);
        }
    }
  
    void Update()
    {
        // Check if the 'E' key is pressed and the player is in range
        if (Input.GetKeyDown(KeyCode.E) && playerInCarRange)
        {
            
            if (!doorStatus) // DOOR IS CLOSED
            {
                carAnimation.SetBool("Door", true);
                audioSource.PlayOneShot(doorOpen);
            }
            else // DOOR IS OPEN
            {
                carAnimation.SetBool("Door", false);
                StartCoroutine(PlaySoundWithDelay(doorClose));
            }

            Debug.Log("Door interaction triggered.");
            doorStatus = !doorStatus;
        }
        else if(Input.GetKeyDown(KeyCode.E) && playerInHoodRange)
        {
            if (!hoodStatus) // Trunk IS CLOSED
            {
                hoodAnimation.SetBool("Hood", true);
                //audioSource.PlayOneShot(doorOpen);
            }
            else // DOOR IS OPEN
            {
                hoodAnimation.SetBool("Hood", false);
                //StartCoroutine(PlaySoundWithDelay(doorClose));
            }
            hoodStatus = !hoodStatus;
        }
        
    }

    IEnumerator PlaySoundWithDelay(AudioClip sound)
    {
        // Wait for a delay before playing the sound
        yield return new WaitForSeconds(0.5f); // Adjust the delay time as needed
        audioSource.PlayOneShot(sound);
        // Play the specified sound
        
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            if (gameObject.CompareTag("CarDoor"))
            {
                Debug.Log("Press 'E' to interact with the door.");
                playerInCarRange = true;
            }
            else
            {
                Debug.Log("Press 'E' to interact with the Hood.");
                playerInHoodRange = true;
            }
  
        }
    }

    void OnTriggerExit(Collider coll)
    {

        if (coll.CompareTag("Player"))
        {
            if (gameObject.CompareTag("CarDoor"))
            {
                Debug.Log("Out of Door Range");
                playerInCarRange = false;
            }
            else 
            {
                Debug.Log("Out of Hood Range");
                playerInHoodRange = false;
            }
        }
        
    }
}
