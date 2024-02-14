using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX = 15f;
    public float sensY = 15f;
    public float maxRotationSpeed = 5f;

    [Tooltip("This should be the player object, so we can rotate the player as the camera moves left and right")]
    public Transform orientation;

    public float maxYRotation = 80f;
    public float minYRotation = -70f;

    float xRotation;
    float yRotation;
    float zRotation;

    public PlayerHealthBar playerHealth;
    public bool dead = false;
    private bool cursorVisible;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if(playerHealth.onTakeDamage == null)
        {
            playerHealth.onTakeDamage = new FloatEvent();
        }
        // whenever the evnnt invoke is called on the event, take damage call back gets called 
        playerHealth.onTakeDamage.AddListener(amIDead);

        //Is the cursor locked in our game view or not
        cursorVisible = Cursor.visible;
    }

    public void amIDead(float health)
    {
        if(health <= 0)
        {
            dead = true;
        }
    }


    private void Update()
    {
        cursorVisible = Cursor.visible;
        /* we add this becuase input.GetAxis was modifying the orignal rotation which was causing the death aniomation to not play properly 
         having this bool prevent the animation from messing up */
        //If our cursor is visible it means we are not locked in our game view and shouldn't move the camera.
        if(dead || cursorVisible)
        {
            return;
        }
        GetDesiredRotation();
        SetCurrentRotation();
    }

    /// <summary>
    /// Get the desired rotation using mouse movement.
    /// </summary>
    private void GetDesiredRotation()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX * 100f;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY * 100f;
        xRotation += mouseX;

        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -maxYRotation, -minYRotation);
    }

    /// <summary>
    /// Sets transform rotations given current desired x and y rotations.
    /// </summary>
    private void SetCurrentRotation()
    {
        Vector3 currentAngle = transform.rotation.eulerAngles;
        float timeScale = Time.deltaTime * 3f;

        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, yRotation, timeScale),
            Mathf.LerpAngle(currentAngle.y, xRotation, timeScale),
            Mathf.LerpAngle(currentAngle.z, zRotation, timeScale));

        transform.eulerAngles = currentAngle;

        //transform.rotation = Quaternion.Euler(yRotation, xRotation, 0);
        orientation.rotation = Quaternion.Euler(0, xRotation, 0);
    }

    public Vector3 GetRotation()
    {
        return new Vector3(xRotation, yRotation, zRotation);
    }

}
