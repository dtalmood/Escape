using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    // we will use this to grab height of player before after after jump
    bool grabInitial = false;
    float jumpInitialHeight;
    float jumpAfterHeight;

    /*[HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;*/

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    // current movement state
    public MovementState state;
    public enum MovementState
    {
        walking, sprinting, air, crouching, idle
    }

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();
        checkFallDamage(); // when we are in the air we want to see how high the palyer has jumped from
        Debug.Log("Player Current State: " + state);
        // handle drag
        /*if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;*/

        if (grounded)
            rb.drag = groundDrag;
        else if (OnSlope())
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && (grounded || OnSlope()) )
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // crouching
        if (Input.GetKeyDown(crouchKey))
        {
            // shrink player down (y scale)
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            // avoids player floating
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouching
        if (Input.GetKeyUp(crouchKey))
        {
            // player height goes back to normal
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

   private void StateHandler()
{
    // player is crouching
    if (Input.GetKey(crouchKey))
    {
        state = MovementState.crouching;
        moveSpeed = crouchSpeed;
    }

    // player is running
    else if (grounded && Input.GetKey(sprintKey))
    {
        state = MovementState.sprinting;
        moveSpeed = sprintSpeed;
    }
    // player is walking
    else if (grounded && (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0))
    {
        state = MovementState.walking;
        moveSpeed = walkSpeed;
    }

    // player is in the air
    else if (!grounded)
    {
        state = MovementState.air;
    }

    // not running or walking and on the ground, so in idle
    else
    {
        state = MovementState.idle;
    }
}

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        // rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // on plane
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
            // down foce when player is walking up slope
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn off gravity when on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // reduce speed on slope
        if (OnSlope() && !exitingSlope)
        {
            // if dist > speed
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }

        // limit speed on ground or air
        else
        {  
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // player will jump higher every time 
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        // player will jump same every time
        // rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }
    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.2f))
        {
            // calculate slope, return angle
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        // ray cast hits nothing
        return false;
    }

    // move direction parallel to slope
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void checkFallDamage()
    {
        if (!grabInitial && state == MovementState.air)
        {
            // Store the initial jump height when the player first jumps
            jumpInitialHeight = transform.position.y;
            grabInitial = true;
            Debug.Log("Initial Height = " + jumpInitialHeight);
        }

        // Check if the player is no longer in the air and in walking or idle state
        if (grabInitial && (state == MovementState.walking || state == MovementState.idle))
        {
            // Store the current height when the player is back on the ground
            jumpAfterHeight = transform.position.y;
            Debug.Log("After Height = " + jumpAfterHeight);
            grabInitial = false; // Reset the flag
        }
    }

    // send info to playerHealthBar to reduce damage 

}

