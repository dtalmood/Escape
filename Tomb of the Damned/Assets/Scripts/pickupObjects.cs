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
    private bool isCarryingObject = false; // Track if an object is being carried
    private GameObject carriedObject; // Reference to the carried object
    private float carriedObjectDistance = 1.8f; // Distance of the carried object from the player
    // do not raise the value above 1.8, if so it will not let you drop item you are holding for some reason????

    // Add a variable to control the throw force
    public float throwForce = 10f;

    void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            objectName = hitObject.name;
            objectTag = hitObject.tag;
            objectDistance = hit.distance;
            //Debug.Log("Looking at: " + objectName);

            objectRigidBodyOrNot = hitObject.GetComponent<Rigidbody>() != null ? "Rigidbody" : "Not Rigidbody";

            pickUpObject(objectName, objectTag, objectRigidBodyOrNot, objectDistance);
        }
        else
        {
            //Debug.Log("Too Far");
        }

        if (isCarryingObject)
        {
            UpdateCarriedObjectPosition();

            // Throw the carried object when the 'Q' key is pressed
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ThrowObject();
            }
        }
    }

    private void pickUpObject(string objName, string objTag, string objRigidBodyOrNot, float objDistance)
    {
        if (objRigidBodyOrNot == "Rigidbody" && objDistance <= 1.3f)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isCarryingObject) // Drop the object
                {
                    DropObject();
                }
                else // Pick up the object
                {
                    carriedObject = GameObject.Find(objName); // Replace with hit.collider.gameObject if needed
                    carriedObject.GetComponent<Rigidbody>().isKinematic = true;
                    isCarryingObject = true;
                    //Debug.Log("Picked up: " + objName);
                }
            }
        }
    }

    private void UpdateCarriedObjectPosition()
    {
        carriedObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward * carriedObjectDistance;
    }

    private void ThrowObject()
    {
        carriedObject.GetComponent<Rigidbody>().isKinematic = false;
        carriedObject.GetComponent<Rigidbody>().velocity = playerCamera.transform.forward * throwForce;
        isCarryingObject = false;
        carriedObject = null;
        //Debug.Log("Threw the Object");
    }

    private void DropObject()
    {
        carriedObject.GetComponent<Rigidbody>().isKinematic = false;
        carriedObject = null;
        isCarryingObject = false;
        //Debug.Log("Dropped the Object");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isCarryingObject)
        {
            // If the carried object collides with another object, consider stopping its movement.
            isCarryingObject = false;
            carriedObject.GetComponent<Rigidbody>().isKinematic = false;
            //Debug.Log("Collision occurred. Dropped the Object to avoid collision.");
        }
    }
}
