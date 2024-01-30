using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncCameraRotationToAnimation : MonoBehaviour
{
    public Transform playerCamera; // Drag and drop your player camera here
    private Animator animator; // Drag and drop your animation controller here

    // Store the initial rotation of the camera
    private Quaternion initialCameraRotation;

    void Start()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player camera reference is not set in the inspector!");
            return;
        }

        // Capture the initial rotation of the camera
        animator = GetComponent<Animator>();

        // Debug logs to check if animator is assigned correctly
        if (animator != null)
        {
            Debug.Log("Animator component found!");
        }
        else
        {
            Debug.LogError("Animator component not found!");
        }

        initialCameraRotation = playerCamera.rotation;
    }

    void Update()
    {
        if (playerCamera != null)
        {
            // Get the rotation of the camera in a way that handles the 0-360 range
            float cameraRotationX = Mathf.DeltaAngle(0f, playerCamera.rotation.eulerAngles.x);
            float cameraRotationY = Mathf.DeltaAngle(0f, playerCamera.rotation.eulerAngles.y);

            // Print the corrected rotation values to the console
            Debug.Log("Camera Rotation X: " + cameraRotationX);
            Debug.Log("Camera Rotation Y: " + cameraRotationY);

            // Pass the rotation values to the animation controller
            if (animator != null)
            {
                animator.SetFloat("Rotation.x (Default Value)", cameraRotationX);
                animator.SetFloat("Rotation.y (Default Value)", cameraRotationY);
            }
            else
            {
                Debug.LogError("Animation controller reference is not set in the inspector!");
            }
        }
        else
        {
            Debug.LogError("Player camera reference is not set in the inspector!");
        }

        // Check if the "=" key is pressed
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            // Trigger the animation
            if (animator != null)
            {
                animator.SetTrigger("PlayAnimation");
            }
            else
            {
                Debug.LogError("Animation controller reference is not set in the inspector!");
            }
        }
    }
}
