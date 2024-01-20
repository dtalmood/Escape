using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;
    public float maxRotationSpeed = 5f;

    public Transform orientation;

    public float maxYRotation;
    public float minYRotation;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX * 100f;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY * 100f;
        xRotation += mouseX;
        
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -maxYRotation, -minYRotation);
        transform.rotation = Quaternion.Euler(yRotation, xRotation, 0);
        orientation.rotation = Quaternion.Euler(0, xRotation, 0);
    }
}
