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

    // THIS IS HOW WE CONNECT PLAYER HEALTH SCRIPT WITH PLAYER MOVEMENT
    public playerHealthBar playerHealth;
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
    public LayerMask ObjectGround; // layer used to detrmine if we are on a 3d object 
    public LayerMask TerrainGround;// layer used to detrmine if we are on a Terrain 
    bool groundedObject; // Tells us if we are on object
    bool groundedTerrain; // tells us if we are on Terrain

     public TerrainDetector terrainDetector;

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
        terrainDetector = GetComponent<TerrainDetector>();
        rb = GetComponentInChildren<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        startYScale = transform.localScale.y;
        audio_Source = transform.Find("Audio Source").GetComponent<AudioSource>();// this will search unity for a component that has specified data inside of it  
        

        // play a jump sound 
        // sound = sandFootSteps.jumpSound; // It take the specific audio clip from the collection and ties it to the audioclip in the player movement script  
        // audio_Source.PlayOneShot(sound);

        // play land sound 
        // sound = sandFootSteps.landSound; // It take the specific audio clip from the collection and ties it to the audioclip in the player movement script  
        // audio_Source.PlayOneShot(sound);

        // play  step sound file 1  
        // sound = sandFootSteps.footStepSounds[0];
        // audio_Source.PlayOneShot(sound);
        
        // play step sound file 2 
        // sound = sandFootSteps.footStepSounds[3];
        // audio_Source.PlayOneShot(sound);

        // Assign the reference to PlayerHealthBar script
        
    }
    

    private void Update()
    {
        // ground check
        groundedObject = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ObjectGround);
        //Debug.Log("Ground: "+ groundedObject);

        groundedTerrain = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, TerrainGround);
        //Debug.Log("Terrain: "+ groundedTerrain);

        MyInput();
        SpeedControl();
        StateHandler();
        fallDamangeCheck();
        playSound();
            
        if (groundedObject || groundedTerrain)
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
        if (Input.GetKey(jumpKey) && readyToJump && ( groundedTerrain || groundedObject || OnSlope()))
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
        if (Input.GetKey(crouchKey) && (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
            //Debug.Log("In Crouch Walking State");
        }

        // player is running
        else if ((groundedTerrain || groundedObject) && Input.GetKey(sprintKey) && (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            //Debug.Log("In Running State");
        }
        // player is walking
        else if ((groundedTerrain || groundedObject) && (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0))
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            //Debug.Log("In Walking State");
        }

        // player is in the air
        else if (!(groundedTerrain || groundedObject))
        {
            state = MovementState.air;   
            //Debug.Log("In air State");
        }

        // not running or walking and on the ground, so in idle
        else
        {
            state = MovementState.idle;
            //Debug.Log("In idle State");
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
        else if ((groundedTerrain || groundedObject))
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!(groundedTerrain || groundedObject))
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


    // from the two functions below it   
    private void fallDamangeCheck()
    {
        if(state == MovementState.air)
        {
            grabInitalHeight();
        }
        else if(grabInitial && state != MovementState.air)
        {
            grabAfterlHeight();
            grabInitial = false;
            playerHealth.fallDamage(jumpInitialHeight,jumpAfterHeight);// go to playerHealthBarScript
        }

    }
    
    // Grabs Player Height the second they are in the air 
    private void grabInitalHeight()
    {
        if(!grabInitial)
        {
            jumpInitialHeight = transform.position.y;
            grabInitial = true;
            //Debug.Log("Before: " + jumpInitialHeight);
            
        }         
    }
    // Grabs player Height the second they reach the ground
    private void grabAfterlHeight()
    {
        jumpAfterHeight =  transform.position.y;  
    }

    // this function will play sound of foot setps when play is walking on differnt terrains
    

    int randomNumber;
    public footStepCollection sandFootSteps; // this object holds the sand sounds 
    public footStepCollection gravelFootSteps; // this object holds gravel sounds 
    public AudioSource audio_Source;

    // this object which will switch the the correct sound and play the proper foot step
    public AudioClip sound; 
    public float footstepDelay = 0.5f; // Adjustable delay between footstep sounds
    
    /*
     We have two layer types currently: 
       - Pebbles_B_TerrainLayer
       - Sand_TerrainLayer
    */  
    
    private void playSound()
    {
        //Debug.Log("Current state in PlaySound: " + state); // Add this line for debugging

        if (state != MovementState.idle) // handle when the player is moving
        {
            if (groundedTerrain) // Player is on terrain 
            {
                string current = terrainDetector.getLayerName(); // grab the name of the terrain I am currently walking on 
                //Debug.Log("On Terrain: " + current);
                
                switch (state) // decide whether to play the walking, sprininting, crouch walking sound 
                {
                     case MovementState.walking:
                        playWalkSound(current);
                        break;
                    
                    }
                
            }
          
            else // Player is on a 3D object or not
            {
                // Player is on a 3D object, get the object name
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * 0.5f + 0.2f, ObjectGround))
                {
                    string objectName = hit.collider.gameObject.name;
                    Debug.Log("On 3D Object: " + objectName);

                    switch (state)
                    {
                        case MovementState.walking:
                            // play walking sound here 
                            // Add code here to swap the walking sound file based on the 3D object
                            // Example: walkingSoundManager.PlaySound(objectName);
                            break;

                        case MovementState.sprinting:
                            // play sprinting sound here 
                            // Add code here to swap the sprinting sound file based on the 3D object
                            // Example: sprintingSoundManager.PlaySound(objectName);
                            break;
                    }
                }
            }


        }


    }

    public void playWalkSound(string current)
    {
        if (current == "Sand_TerrainLayer")
        {
            Debug.Log("Play Sand Sound");
            randomNumber = Random.Range(0, 4);
            sound = sandFootSteps.footStepSounds[randomNumber];

            // Play footstep sound with adjustable delay
            StartCoroutine(PlayFootstepWithDelay(sound, footstepDelay));
        }
        else if (current == "Pebbles_B_TerrainLayer")
        {
            Debug.Log("Play Pebbles Sound");
        }
    }

    // Coroutine to play footstep sound with adjustable delay
    private IEnumerator PlayFootstepWithDelay(AudioClip footstepSound, float delay)
    {
        // Play the footstep sound
        audio_Source.PlayOneShot(footstepSound);

        // Wait for the adjustable delay before allowing the next footstep sound
        yield return new WaitForSeconds(delay);

        // Continue with the rest of the code or actions you want to perform after the delay
        // For example, you can add more logic here or call another function
    }


    // public AudioClip swapFootStep(string current)
    // {
    //     if(current == )
         
    // }
    // public void playJumpSound(AudioClip soundFile)
    // {
    //     sound = sandFootSteps.jumpSound; // It take the specific audio clip from the collection and ties it to the audioclip in the player movement script  
    //     audio_Source.PlayOneShot(soundFile);
    // }


}

