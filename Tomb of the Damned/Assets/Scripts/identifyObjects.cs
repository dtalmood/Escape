using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class identifyObjects : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 10f;

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            string objectName = hitObject.name;
            string objectTag = hitObject.tag;

            Debug.Log("Looking at: " + objectName);
        }
    }
}
