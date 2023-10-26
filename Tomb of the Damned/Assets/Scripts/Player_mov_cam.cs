using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    // walk, run, jump, grav
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    // crouching
    bool isCrouching;
    public float crouchSpeed = 3f;
    public float crouchYscale;
    private float startYScale;
    public KeyCode crouchKey = KeyCode.LeftControl;
 
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    Rigidbody rb;

    // stores current movement state
    public MovementState state;

    // movement state
    public enum MovementState
    {
        walking,
        running,
        crouching,
        air
    }

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
 
    public bool canMove = true;
 
    
    CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // set start y scale
        startYScale = transform.localScale.y;
    }
 
    void Update()
    {
        #region Handles Movment
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
 
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        // set state to run
        if (isRunning)
        {
            state = MovementState.running;
            Debug.Log("Player is running");
        }

        // crouching 
        if (Input.GetKeyDown(crouchKey))
        {
            // change y
            transform.localScale = new Vector3(transform.localScale.x, crouchYscale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            // set crouch state
            state = MovementState.crouching;
            curSpeedX = crouchSpeed;
            isCrouching = true;
        }
        
        // stop crouching
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            isCrouching = false;
            state = MovementState.walking;
        }

        #endregion
 
        #region Handles Jumping
        bool isJumping;
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded && !isCrouching)
        {
            isJumping = true;
            moveDirection.y = jumpPower;
            // set jump state
            state = MovementState.air;
            Debug.Log("Player is jumping");
        }
        else
        {
            isJumping = false;
            moveDirection.y = movementDirectionY;
        }
 
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        
        // set walk state
        if (!isJumping && !isRunning && !isCrouching)
        {
            state = MovementState.walking;
            Debug.Log("Player is walking");
        }
        
 
        #endregion
 
        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);
 
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
 
        #endregion
    }
}