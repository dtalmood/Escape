using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;

public class FadeSound : MonoBehaviour
{
    List <AudioClip> myClips;
    GameObject player;
    private AudioSource audioSource; 
    public TerrainDetector detector;
    public LayerMask ObjectGround; // layer used to detrmine if we are on a 3d object 
    bool groundedObject; // Tells us if we are on object
    RaycastHit hit;
    string current;

    AudioSourcePool sourcePool;



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = transform.Find("Audio Source").GetComponent<AudioSource>();
        detector = player.GetComponent<TerrainDetector>();
        sourcePool = gameObject.AddComponent<AudioSourcePool>();
        sourcePool.onPlayAudioSource ??= new AudioSourceEvent();
        
        sourcePool.onPlayAudioSource.AddListener(AudioSourcePlayedCallback);
    }

    public void AudioSourcePlayedCallback(AudioSource source)
    {
        AudioClip clip = source.clip;

       if (audioSource == null)
       {
            Debug.Log("audio source is null");
            return;
       }

        if (source.clip == null)
        {
            Debug.Log("audio source CLIP is null");
        }
        //Debug.Log($"Called hook. onPlayAudioSource with clip: {source.clip.name}");
        
    }

    // Method to fade in sounds such as player running, heartbeat, etc . . . 
    public void fadeInSoundEffects(AudioClip clip, float initialVolume = 0)
    {
        //Play the clip
        AudioSource source = sourcePool.PlayClip(clip, initialVolume);
        StartCoroutine(FadeAudio(clip, source));
    }

    // Method to fade in Music when player is outside 
    public void fadeInOutsideMusic(AudioClip clip)
    {
        Debug.Log("Play Sound");
        audioSource.clip = clip;
        audioSource.Play();
    }

    
    // Method to fade in music when player is indoors
    public void fadeInIndoorMusic(AudioClip clip)
    {
        
    }

    // method to fade in music when player is being chased by the monster 
    public void fadeInChaseMusic(AudioClip clip)
    {
        StartCoroutine(FadeAudio(clip, audioSource));
    }

    // Function that fades in Audio and plays needed Sound Effect
    IEnumerator FadeAudio(AudioClip clip, AudioSource source, float initialVolume = 0)
    {
        Debug.Log(source);
        
        myClips ??= new List<AudioClip>();
        if(myClips.Contains(clip))// Checks if the audio clip we are attempting to paly is already playig 
        {
            Debug.Log($"my clips contains audio clip already: {clip.name}");
            yield break;
        }

        myClips.Add(clip);
        float targetVolume = 1.0f; // Assuming fade in effect
        // Set clip and play
        source.volume = initialVolume;
        if(source.clip != clip)
        {
            source.clip = clip;
        }

        if (!source.isPlaying)
        {
            source.Play();
        }

        // Fade in
        while (source.volume < targetVolume)
        {
            source.volume += Time.deltaTime / 2f; // Adjust the time duration as needed
            yield return null;
        }
    }
}
