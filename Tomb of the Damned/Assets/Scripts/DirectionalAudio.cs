using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DirectionalAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public Transform circularAreaCenter;
    public float maxRadius = 5f; // Replace with your actual maximum radius

    void Update()
    {
        // Example usage: call this function with the actual position
        CheckAudioPlay(transform.position);
    }

    void CheckAudioPlay(Vector3 position)
    {
        // Calculate distance from the circular area center
        float distance = Vector3.Distance(position, circularAreaCenter.position);

        // Calculate the allowed radius (half of the circular area)
        float allowedRadius = maxRadius / 2;

        // Check if the distance is within the allowed radius
        if (distance <= allowedRadius)
        {
            // Play the audio
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // Pause the audio or handle as needed
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }
}