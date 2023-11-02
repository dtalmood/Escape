using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class identifyObjects : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 1f;

    public string objectName;
    public string objectTag;
    public string objectRigidBodyOrNot;

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // if statement checks if we are looking at a game object
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            objectName = hitObject.name;
            objectTag = hitObject.tag;
            Debug.Log("Looking at: " + objectName);

            // Check if the object has a Rigidbody
            objectRigidBodyOrNot = hitObject.GetComponent<Rigidbody>() != null ? "Rigidbody" : "Not Rigidbody";

            // Send data to the script that handles picking up objects such as gold
            pickingUpObjects.recieveObjectName = objectName;
            pickingUpObjects.recieveObjectTag = objectTag;
            pickingUpObjects.recieveObjectRigidBodyOrNot = objectRigidBodyOrNot;
        }
    }
}
