using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SyncCameraRotationToAnimation : MonoBehaviour
{
    public Transform playerCamera; // Drag and drop your player camera here
    private Animator animator; // Drag and drop your animation controller here
    
    public PlayerHealthBar playerHealth;

    // Store the initial rotation of the camera
    private Quaternion initialCameraRotation;

    void Start()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player camera reference is not set in the inspector!");
            return;
        }

        // By defualt unity Events are Null
        // we want the event of Playe Death to happen what? When the Health reaches 0 
        if(playerHealth.onTakeDamage == null)
        {
            // multiple scripts are doing this potentially, so anyone that runs this should make sure its not assgined 
            playerHealth.onTakeDamage = new FloatEvent();
        }
        // 
        playerHealth.onTakeDamage.AddListener(TakeDamageCallback);
    
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

    public void TakeDamageCallback(float health)
    {
        // player is not dead 
        if(health > 0)
        {
            return;
        }
        // Player has Died 
        // In Animation Called Death We have a boolean Called Dead 
        // We set Boolean Dead = True 
        // This will call even Death inside of the Anmation Controller 
        animator.SetBool("Dead", true);
        animator.SetBool("Fade", true);
    }
}
