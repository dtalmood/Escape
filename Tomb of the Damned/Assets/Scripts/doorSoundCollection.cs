using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new door Collection", menuName = "create new door Collection ")]


public class doorSoundCollection : ScriptableObject
{
   public List<AudioClip> doorSounds = new List<AudioClip>();
   
}