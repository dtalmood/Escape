using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Events;

public class AudioSourceEvent : UnityEvent<AudioSource>{}

public class AudioSourcePool : MonoBehaviour
{
    public AudioSourceEvent onPlayAudioSource;

    public List<AudioSource> availableSources;
    public List<AudioSource> activeSources;
   
   public void PlayClip(AudioClip clip, float volume)
    {
        InitLists();
        if(!PlayingClip(clip))
        {
            AudioSource source;
            //If we have no available sources, create a new one.
            if (availableSources.Count == 0)
            {
                source = gameObject.AddComponent<AudioSource>();
            }
            //If we do have available sources, we consume it (remove it from the list of available sources)
            else
            {
                source = availableSources[0];
                availableSources.Remove(source);
            }


            if (clip == null)
            {
                Debug.Log("Clip was null in PlayClipEnumerator");
                UnityEditor.EditorApplication.isPaused = true;
            }

            //Play the clip at the volume
            source.volume = volume;
            source.clip = clip;
            source.Play();
            activeSources.Add(source);
            onPlayAudioSource?.Invoke(source);
            StartCoroutine(PlayClipEnumerator(source, volume));
        }
    }
    
    private void InitLists()
    {
        // ??= this is null coralesasing operator 
        // If AudioSources is Null It runs code on the right of ?? 
        // if AudioSources is not null then it assignes AudioSources to iteself 
        //availableSources = availableSources ?? new List<AudioSource>();
        availableSources ??= new List<AudioSource>();
        activeSources ??= new List<AudioSource>();
    }

 

    private bool PlayingClip(AudioClip clip)
    {
        for(int i =0; i < activeSources.Count; i++)
        {
            if(activeSources[i].clip == clip && activeSources[i].isPlaying)
            {
                //Debug.Log($"Already Playing sound effect {clip.name}");
                return true;
            }
        }
        return false;
    }

    private IEnumerator PlayClipEnumerator(AudioSource source, float volume)
    {
        //Wait until the source is done playing
        while(source.time < source.clip.length || source.isPlaying)
        {
            yield return null;
        }


        Debug.Log("Stopping audio source");
        //remove it from the active sources and put it back into available sources (stop it just in case)
        source.Stop();
        source.clip = null;
        activeSources.Remove(source);
        availableSources.Add(source);


        yield return null;
    }
}
