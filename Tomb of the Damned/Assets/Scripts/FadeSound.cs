using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSound : MonoBehaviour
{
    GameObject player;
    private AudioSource audioSource; 
    public TerrainDetector detector;
    public LayerMask ObjectGround; // layer used to detrmine if we are on a 3d object 
    bool groundedObject; // Tells us if we are on object
    RaycastHit hit;
    string current;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = transform.Find("Audio Source Near Monster").GetComponent<AudioSource>();
        detector = player.GetComponent<TerrainDetector>();
    }

    // Method to fade in sounds such as player running, heartbeat, etc . . . 
    public void fadeInSoundEffects(AudioClip clip)
    {
        StartCoroutine(FadeAudio(clip));
    }

    // Method to fade in Music when player is outside 
    public void fadeInOutsideMusic(AudioClip clip)
    {
        groundedObject = Physics.Raycast(transform.position, Vector3.down, 2 * 0.18f + 0.1f, ObjectGround);
        
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2 * 0.5f + 0.2f, ObjectGround))
        {
            current  = hit.collider.gameObject.tag;
            Debug.Log("Current = " +current);
            if(current != "OutsideWood")
            {
                Debug.Log("Play Sound");
                StartCoroutine(FadeAudio(clip));
            }
        }
        
    }
    
    // Method to fade in music when player is indoors
    public void fadeInIndoorMusic(AudioClip clip)
    {
        
    }

    // method to fade in music when player is being chased by the monster 
    public void fadeInChaseMusic(AudioClip clip)
    {
        StartCoroutine(FadeAudio(clip));
    }

    // Function that fades in Audio and plays needed Sound Effect
    IEnumerator FadeAudio(AudioClip clip)
    {

        float startVolume = audioSource.volume;
        float targetVolume = 1.0f; // Assuming fade in effect

        // Fade in
        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += Time.deltaTime / 2f; // Adjust the time duration as needed
            yield return null;
        }

        // Set clip and play
        audioSource.clip = clip;
        audioSource.Play();
    }

    // 
    
}
