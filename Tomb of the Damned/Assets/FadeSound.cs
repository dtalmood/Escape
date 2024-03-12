using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSound : MonoBehaviour
{
    GameObject player;
    private AudioSource audioSource; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = transform.Find("Audio Source Near Monster").GetComponent<AudioSource>();
    }

    // Method to play sound with fade effect
    public void fadeInAudio(AudioClip clip)
    {
        StartCoroutine(FadeAudio(clip));
    }

    // Coroutine for fading audio
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
}
