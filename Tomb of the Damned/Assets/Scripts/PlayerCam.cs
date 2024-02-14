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

    private Dictionary<string, Vector3> rotationOffets;

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
        //This line is not working so I am temporarily ignoring this functionality
        cursorVisible = false;//Cursor.visible;
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
        //Get current rotation of the object
        Vector3 currentAngle = transform.rotation.eulerAngles;
        //The speed at which we follow with the camera
        float timeScale = Time.deltaTime * 4f;

        Vector3 desiredRotation = new Vector3(yRotation, xRotation, zRotation) + GetCumulativeOffsets();
        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, desiredRotation.x, timeScale),
            Mathf.LerpAngle(currentAngle.y, desiredRotation.y, timeScale),
            Mathf.LerpAngle(currentAngle.z, desiredRotation.z, timeScale));

        transform.eulerAngles = currentAngle;
        orientation.rotation = Quaternion.Euler(0, xRotation, 0);
    }

    public Vector3 GetRotation()
    {
        return new Vector3(xRotation, yRotation, zRotation);
    }

    public Vector3 GetCumulativeOffsets()
    {
        //null-coalescing operator ??= assigns rotationOffsets to the right-hand side operand IFF it is null. 
        //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator
        rotationOffets ??= new Dictionary<string, Vector3>();

        Vector3 cumulativeOffset = Vector3.zero;
        //Add each value in the dictionary if that value is not null. Otherwise just add (0,0,0)
        foreach(Vector3 vec in rotationOffets.Values)
        {
            cumulativeOffset += (vec == null ? Vector3.zero : vec);
        }

        return cumulativeOffset;
    }

    public void AddOrSetRotationOffset(string offsetName, Vector3 offset)
    {
        //null-coalescing operator ??= assigns rotationOffsets to the right-hand side operand IFF it is null. 
        //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator
        rotationOffets ??= new Dictionary<string, Vector3>();

        if(rotationOffets.ContainsKey(offsetName))
        {
            rotationOffets[offsetName] = offset;
        }
        else
        {
            rotationOffets.Add(offsetName, offset);
        }
    }

    public void RemoveRotationOffset(string offsetName)
    {
        //null-coalescing operator ??= assigns rotationOffsets to the right-hand side operand IFF it is null. 
        //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator
        rotationOffets ??= new Dictionary<string, Vector3>();

        if (rotationOffets.ContainsKey(offsetName))
        {
            rotationOffets.Remove(offsetName);
        }
    }

}
