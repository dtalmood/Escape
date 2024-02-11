using System.Collections;
using UnityEngine;

public class CarDoor : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip doorOpen;
    public AudioClip doorOpenChime;
    public AudioClip doorClose;
    public AudioClip hoodOpen;
    public AudioClip hoodClose;
    private bool playerInCarRange = false;
    private bool playerInHoodRange = false;
    private bool doorStatus = false; // False = Close, True = Open
    private bool hoodStatus = false;
    public Animator carAnimation;
    public Animator hoodAnimation;

    private Coroutine chimeCoroutine; // Coroutine reference for the chime loop

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

        if (Input.GetKeyDown(KeyCode.E) && playerInCarRange) // Player is standing near Car Door
        {

            if (!doorStatus) // DOOR IS CLOSED SO OPEN IT
            {
                carAnimation.SetBool("Door", true);
                audioSource.PlayOneShot(doorOpen);
                doorStatus = true;
                // Start the chime loop when the door is opened
                chimeCoroutine = StartCoroutine(ChimeLoop());
            }
            else // DOOR IS OPEN SO CLOSE IT 
            {
                carAnimation.SetBool("Door", false);
                StartCoroutine(PlaySoundWithDelay(doorClose, 0.4f));
                doorStatus = false;
                // Stop the chime loop when the door is closed
                StopCoroutine(chimeCoroutine);
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && playerInHoodRange) // Player is standing in front of the hood of the car
        {
            if (!hoodStatus) // Hood IS CLOSED
            {
                hoodAnimation.SetBool("Hood", true);
                audioSource.PlayOneShot(hoodOpen);
                hoodStatus = true;
            }
            else // HOOD IS OPEN
            {
                hoodAnimation.SetBool("Hood", false);
                StartCoroutine(PlaySoundWithDelay(hoodClose, 0.2f));
                hoodStatus = false;
            }
        }
    }

    IEnumerator PlaySoundWithDelay(AudioClip sound, float delayAmount)
    {
        // Wait for a delay before playing the sound
        yield return new WaitForSeconds(delayAmount); // Adjust the delay time as needed
        audioSource.PlayOneShot(sound);
        // Play the specified sound

    }

    IEnumerator ChimeLoop()
    {
        while (doorStatus) // Loop as long as the door is open
        {
            audioSource.PlayOneShot(doorOpenChime);
            yield return new WaitForSeconds(doorOpenChime.length); // Wait for the chime sound to finish before playing it again
        }
        audioSource.Stop();
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
