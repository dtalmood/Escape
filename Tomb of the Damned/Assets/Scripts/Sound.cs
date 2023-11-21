using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioClip rocks;
    public AudioClip grass;
    
    RaycastHit hit;
    public Transform RayStart; // start position for ray cast 
    public float range; // max range for ray cast 
    public LayerMask layerMask; //

   
}
