using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new FootStep Collection", menuName = "create new Footstep Collection ")]
public class footStepCollection : ScriptableObject
{
   public List<AudioClip> footStepSounds = new List<AudioClip>();
   public AudioClip jumpSound;
   public AudioClip landSound;
}