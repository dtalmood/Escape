using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float sensX;
    public float sensY;
    /*
    Puting Public means that you can adjust its value 
    in the unity Editor without modifting the code itself. 
    */

    public Transform orientation;
    // Transform represents the position, rotation and scale of GameObject
    // Orientation is custom name 

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 
        Cursor.visible = false; // Make Cursor Invisible 
    }

    //Called once per frame
    private void Update()
    {
        //Check Mouse Input 
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation,-90f,90);

        //rotate camera and orientatoin
        transform.rotation = Quaternion.Euler(xRotation, yRotation,0);
        orientation.rotation = Quaternion.Euler(0, yRotation,0);
    }
}
