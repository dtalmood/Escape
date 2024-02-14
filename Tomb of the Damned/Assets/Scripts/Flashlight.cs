using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public PlayerCam playerCam;
    public Light flashLight;

    public void Start()
    {
        if(flashLight == null)
        {
            flashLight = GetComponent<Light>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camRotation = playerCam.GetRotation();
        transform.rotation = Quaternion.Euler(camRotation.y, camRotation.x, 0);

        if(Input.GetKeyDown("f"))
        {
            flashLight.enabled = !flashLight.enabled;
        }
    }
}
