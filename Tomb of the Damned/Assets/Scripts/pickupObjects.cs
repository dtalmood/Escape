using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifyObjects : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 0.1f;
    private string objectName; // Object Name 
    private string objectTag; // Object Type 
    private string objectRigidBodyOrNot; // Identifies if object is a rigid body
    private float objectDistance; // checks how far away the object is from the player 
    


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
            objectDistance = hit.distance;
            Debug.Log("Looking at: " + objectName);

            // Check if the object has a Rigidbody
            objectRigidBodyOrNot = hitObject.GetComponent<Rigidbody>() != null ? "Rigidbody" : "Not Rigidbody";

            // Call the pickUpObject method with the object's data
            pickUpObject(objectName, objectTag, objectRigidBodyOrNot, objectDistance);
        }
        else
        {
            Debug.Log("Too Far");
        }
    }

    private void pickUpObject(string objName, string objTag, string objRigidBodyOrNot, float objDistance)
    {
        if (objRigidBodyOrNot == "Rigidbody" && objDistance <= 1.3f)
        {
            //Debug.Log("We can pick up this Object");
            if(Input.GetKeyDown(KeyCode.E))
            {

            }

        }
        else
        {
            Debug.Log("We cannot pick up this Object");
        }
    }
}
