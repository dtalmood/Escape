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
        // sound = grassFootSteps.jumpSound; // It take the specific audio clip from the collection and ties it to the audioclip in the player movement script  
        // audio_Source.PlayOneShot(sound);

        // play land sound 
        // sound = grassFootSteps.landSound; // It take the specific audio clip from the collection and ties it to the audioclip in the player movement script  
        // audio_Source.PlayOneShot(sound);

        // play  step sound file 1  
        // sound = grassFootSteps.footStepSounds[0];
        // audio_Source.PlayOneShot(sound);
        
        // play step sound file 2 
        // sound = grassFootSteps.footStepSounds[3];
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


    public AudioSource audio_Source; // this Game Object will be used to switch the the correct sound and play the proper foot step
    public AudioClip sound; 
    string current;
    bool jump;
    private float walkSoundDelay = 0.7f; // how much to delay sound between footsteps when walking 
    private float sprintSoundDelay = 0.6f; // how much to delay sound between footsteps when sprinting
    private float crouchSoundDelay = 0.9f; // when crouch walking
    private float landSoundDelay = 0.5f;

    private void playSound()
    {
        //Debug.Log("Current state in PlaySound: " + state); // Add this line for debugging

        if (state != MovementState.air) // handle when the player is moving
        {
            current = terrainDetector.getLayerName(); // grab the name of the terrain I am currently walking on

            // This if statemnet handles when the player lands 
            if(!jump && (groundedTerrain || groundedObject)) // 
            {
                Debug.Log("Enter");
                jump = true;
                playLandSound(current);
            }
            
            
            if (groundedTerrain) // Player is on terrain 
            {
                
                switch (state) // decide whether to play the walking, sprininting, crouch walking sound 
                {
                    case MovementState.walking:
                    playWalkSprintCrouchSound(current, walkSoundDelay);
                    break;

                    case MovementState.sprinting:
                    playWalkSprintCrouchSound(current, sprintSoundDelay);
                    break;    

                    case MovementState.crouching:
                    playWalkSprintCrouchSound(current, crouchSoundDelay);
                    break;
                
                }
                
            }
          
            else if(groundedObject)// Player is on a 3D object or not
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * 0.5f + 0.2f, ObjectGround))
                {
                    string objectName = hit.collider.gameObject.name;
                    Debug.Log("On 3D Object: " + objectName);

                    switch (state)
                    {
                        case MovementState.walking:
                            break;

                        case MovementState.sprinting:
                            break;
                    }
                }
            }
            
        }
        else if(state == MovementState.air)
        {
            //Debug.Log("In Air State");
            current = terrainDetector.getLayerName();
            playJumpSound(current);
        }

    }


    int randomNumber;
    public footStepCollection grassFootSteps; // this object holds the Grass sounds 
    public footStepCollection gravelFootSteps; // this object holds Gravel sounds 

    bool play = true;

    public void playWalkSprintCrouchSound(string current, float delayAmount)
    {
        if (current == "Terrain_Layer4_Grass_Plants")
        {
            if (play)
            {
                randomNumber = Random.Range(0, 4);
                sound = grassFootSteps.footStepSounds[randomNumber];
                play = false;      
                StartCoroutine(Delay(sound, delayAmount)); // Play footstep sound with adjustable delay
            }
        }
        else if (current == "Pebbles_B_TerrainLayer")
        {
            if (play)
            {
                randomNumber = Random.Range(0, 4);
                sound = gravelFootSteps.footStepSounds[randomNumber];
                play = false;
                StartCoroutine(Delay(sound, delayAmount)); // Play footstep sound with adjustable delay
            }
        }
    }
    

    private IEnumerator Delay(AudioClip sound, float delay)
    {
        // Play the footstep sound
        audio_Source.PlayOneShot(sound);
        // Wait for the adjustable delay before allowing the next footstep sound
        yield return new WaitForSeconds(delay);
        play = true;
    }


    
    private void playJumpSound(string current)
    {
        if(jump)
        {
            if (current == "Terrain_Layer4_Grass_Plants")
            {
                Debug.Log("Play Sand Jump");
                sound = grassFootSteps.jumpSound;
                audio_Source.PlayOneShot(sound);
            }

            else if (current == "Pebbles_B_TerrainLayer")
            {
                Debug.Log("Play Pebble Jump");
                sound = gravelFootSteps.jumpSound;
                audio_Source.PlayOneShot(sound);
                
            }
            jump = false;
        }
       
    }

    private void playLandSound(string current)
    {
        if (current == "Terrain_Layer4_Grass_Plants")
        {
            Debug.Log("Play Sand Land");
            sound = grassFootSteps.landSound;
            StartCoroutine(Delay(sound, landSoundDelay));
        }

        else if (current == "Pebbles_B_TerrainLayer")
         {
             Debug.Log("Play Pebble Land");
             sound = gravelFootSteps.landSound;
             StartCoroutine(Delay(sound, landSoundDelay));
        }
            
    }



}