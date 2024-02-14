using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public PlayerCam playerCam;
 
    // Update is called once per frame
    void Update()
    {
        Vector3 camRotation = playerCam.GetRotation();
        transform.rotation = Quaternion.Euler(camRotation.y, camRotation.x, 0);
    }
}
