using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX = 15f;
    public float sensY = 15f;
    public float maxRotationSpeed = 5f;

    public Transform orientation;

    public float maxYRotation = 80f;
    public float minYRotation = -70f;

    float xRotation;
    float yRotation;

    public PlayerHealthBar playerHealth;
    public bool dead = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if(playerHealth.onTakeDamage == null)
        {
            playerHealth.onTakeDamage = new FloatEvent();
        }
        // whenever the evnnt invoke is called on the event, take damage call back gets called 
        playerHealth.onTakeDamage.AddListener(TakeDamageCallback);
    }

    public void TakeDamageCallback(float health)
    {
        if(health <= 0)
        {
            dead = true;
        }
    }


    private void Update()
    {
        // we add this becuase input.GetAxis was modifying the orignal rotation which was causing the death aniomation to not play properly 
        // having this bool prevent the animation from messing up 
        if(dead)
        {
            return;
        }
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX * 100f;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY * 100f;
        xRotation += mouseX;
        
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -maxYRotation, -minYRotation);
        transform.rotation = Quaternion.Euler(yRotation, xRotation, 0);
        orientation.rotation = Quaternion.Euler(0, xRotation, 0);
    }
}
