



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerMovement : MonoBehaviour
{
    public PlayerMovementAnimations movementAnimations;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float speedGoingDownSlope = 10f;
    public float speedGoingUpSlope = 10f;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    // THIS IS HOW WE CONNECT PLAYER HEALTH SCRIPT WITH PLAYER MOVEMENT
    public PlayerHealthBar playerHealth;
    public PlayerCam playerCam;
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

    public bool dead = false;

    // current movement state
    public MovementState state; 
    public enum MovementState
    {
        walking, sprinting, air, crouching, idle, backwards
    }

    private void Start()
    {
        terrainDetector = GetComponent<TerrainDetector>();
        rb = GetComponentInChildren<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        startYScale = transform.localScale.y;
        audio_Source = transform.Find("Audio Source").GetComponent<AudioSource>();// this will search unity for a component that has specified data inside of it  
        

        if(playerHealth.onTakeDamage == null)
        {
            playerHealth.onTakeDamage = new FloatEvent();
        }
        /*
         We have the invoke inside of PlayerHeathBar Script 
         On Line 61:  onTakeDamage?.Invoke(currentHealth);" 
         When Line 61 is called it looks for on Take Damage Listeneer 
         The Compiler then finds this Listener and Says HEY LOOK A LISTNER 
         The Listener Then Tell Unity "Hey Run the Function AmiDead"
        */
        playerHealth.onTakeDamage.AddListener(AmIDead);
    }

    public void AmIDead(float health)
    {
        if(health <= 0)
        {
            dead = true;
            moveSpeed = 0;
            rb.velocity = Vector3.zero;
        }
    }
    

    private void Update()
    {
        // if this is true this return statment ensures the palyer will stop
        if(dead)
            return;
        // ground check
        groundedObject = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.18f + 0.1f, ObjectGround);
        //Debug.Log("Ground: "+ groundedObject);

        groundedTerrain = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.1f + 0.1f, TerrainGround);
        groundedTerrain = !groundedObject && groundedTerrain; 
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

    public void PlayerCamMovementOffset(Vector3 input)
    {
        float maxZRotation = 2f;
        float zRotation = input.x == 0 ? 0 : -input.x * maxZRotation;
        Vector3 rotation = new Vector3(0, 0, zRotation);

        playerCam.AddOrSetRotationOffset("HorizontalMovementRotation", rotation);
    }


    private void MyInput()
    {

        if(dead)
            return;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        PlayerCamMovementOffset(new Vector3(horizontalInput, verticalInput, 0));
        

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

    // STATE MACHINE 
   private void StateHandler()
    {
        // player is crouching
        if (Input.GetKey(crouchKey) && (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0))
        {
            movementAnimations.crouchWalkForwardAnimation();
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
            //Debug.Log("In Crouch Walking State");
        }

        // player is crouch walking forward
        /*else if (Input.GetKey(crouchKey) && verticalInput > 0)
        {
            movementAnimations.crouchWalkingForwardAnimation();
            state = MovementState.crouchWalkingForward;
            moveSpeed = crouchWalkSpeed;
        }

        // player is crouch walking backward
        else if (Input.GetKey(crouchKey) && verticalInput < 0)
        {
            movementAnimations.crouchWalkingBackwardAnimation();
            state = MovementState.crouchWalkingBackward;
            moveSpeed = crouchWalkSpeed;
            // Debug.Log("In Crouch Walking Backward State");
        }*/

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
            // walking forward
            if (verticalInput > 0)
            {
                movementAnimations.walkingAnimation();
                state = MovementState.walking;
                moveSpeed = walkSpeed;
            }
            // walking backwards
            else if (verticalInput < 0)
            {
                movementAnimations.walkingBackwardsAnimation();
                state = MovementState.backwards;
                moveSpeed = walkSpeed;
            }
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
            movementAnimations.idleAnimation();
            state = MovementState.idle;
            //Debug.Log("In idle State");
        }
    }

    private void MovePlayer()
    {
    // calculate movement direction
    moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

    // on plane
    if (OnSlope() && !exitingSlope)
    {
        Vector3 slopeMoveDirection = GetSlopeMoveDirection();

        // Apply the adjusted force
        rb.AddForce(slopeMoveDirection * moveSpeed * 55f, ForceMode.Force);

        // Apply additional downward force when moving up the slope
        if (rb.velocity.y > 0)
        {
            //Debug.Log("Going up a SLope");
            rb.AddForce(Vector3.down * speedGoingUpSlope, ForceMode.Force);
        }
        // Slow down the descent when moving down the slope
        else if (rb.velocity.y < 0)
        {
            //Debug.Log("Going down a SLope");
            rb.AddForce(Vector3.down * speedGoingDownSlope, ForceMode.Force); // Adjust this value
        }
    }
    // on ground
    else if ((groundedTerrain || groundedObject))
    {
        // Directly apply force in the moveDirection to move straight
        rb.AddForce(moveDirection.normalized * moveSpeed * 8f, ForceMode.Force); // Adjust this value
    }
    // in air
    else if (!(groundedTerrain || groundedObject))
    {
        // Directly apply force in the moveDirection to move straight
        rb.AddForce(moveDirection.normalized * moveSpeed * 8f * airMultiplier, ForceMode.Force); // Adjust this value
    }
    rb.useGravity = !OnSlope();

    // turn off gravity when on slope
    
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
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        return Vector3.ProjectOnPlane(inputDir, slopeHit.normal).normalized;
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
            playerHealth.fallDamage(jumpInitialHeight,jumpAfterHeight);// we have inital Height and After Height and we call playerHEalthBarScript
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
    RaycastHit hit;

    private void playSound()
    {
        //Debug.Log("Current state in PlaySound: " + state); // Add this line for debugging

        if (state != MovementState.air) // handle when the player is moving
        {
            current = terrainDetector.getLayerName(); // grab the name of the terrain I am currently walking on   
            
            // This if statemnet handles when the player lands 
            if(!jump && (groundedTerrain || groundedObject)) // 
            {
                jump = true;
                if(groundedObject)
                {
                    Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * 0.5f + 0.2f, ObjectGround);
                    current  = hit.collider.gameObject.tag;
                }
                playLandSound(current);
                return;
            }
            
            if (groundedTerrain) // Player is on terrain 
            {
                switch (state) // decide whether to play the walking, sprininting, crouch walking sound 
                {
                    case MovementState.walking:
                    terrainPlayWalkSprintCrouchSound(current, walkSoundDelay);
                    return;
                    
                    case MovementState.backwards:
                    terrainPlayWalkSprintCrouchSound(current, walkSoundDelay);
                    return;

                    case MovementState.sprinting:
                    terrainPlayWalkSprintCrouchSound(current, sprintSoundDelay);
                    return;    

                    case MovementState.crouching:
                    terrainPlayWalkSprintCrouchSound(current, crouchSoundDelay);
                    return;
                
                }
                
            }
          
            else if(groundedObject)// Player is on a 3D object or not
            {
                //Debug.Log("On Object");
                if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * 0.5f + 0.2f, ObjectGround))
                {
                    current  = hit.collider.gameObject.tag;

                    //Debug.Log("On 3D Object: " + current);

                    switch (state) // decide whether to play the walking, sprininting, crouch walking sound 
                    {
                        case MovementState.walking:
                        objectPlayWalkSprintCrouchSound(current, walkSoundDelay);
                        return;

                        case MovementState.backwards:
                        objectPlayWalkSprintCrouchSound(current, walkSoundDelay);
                        return;

                        case MovementState.sprinting:
                        objectPlayWalkSprintCrouchSound(current, sprintSoundDelay);
                        return;    

                        case MovementState.crouching:
                        objectPlayWalkSprintCrouchSound(current, crouchSoundDelay);
                        return;
                    
                    }
                }
            }
            
        }
        else if(state == MovementState.air)
        {
            //Debug.Log("Current terrain/object: " + current);
            //current = terrainDetector.getLayerName();
            playJumpSound(current);
        }

    }


    int randomNumber;
    public footStepCollection grassFootSteps; // this object holds the sand sounds 
    public footStepCollection gravelFootSteps; // this object holds gravel sounds 
    public footStepCollection woodFootSteps; // this object holds the sand sounds 
    public footStepCollection tileFootSteps; // this object holds gravel sounds
    public footStepCollection concreteFootSteps; // this object holds gravel sounds 
    bool play = true;

    // this handles playing sound when walking on a terrain 
    public void terrainPlayWalkSprintCrouchSound(string current, float delayAmount)
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
    
    // this handles playing sound when walking on a 3D object 
    public void objectPlayWalkSprintCrouchSound(string current, float delayAmount)
    {
        if(current == "Wood" || current == "OutsideWood")
        {
            if (play)
            {
                randomNumber = Random.Range(0, 4);
                sound = woodFootSteps.footStepSounds[randomNumber];
                play = false;
                StartCoroutine(Delay(sound, delayAmount)); // Play footstep sound with adjustable delay
            }
        }
        else if(current == "Tile")
        {
            if (play)
            {
                randomNumber = Random.Range(0, 4);
                sound = tileFootSteps.footStepSounds[randomNumber];
                play = false;
                StartCoroutine(Delay(sound, delayAmount)); // Play footstep sound with adjustable delay
            }

        }
        else if(current == "Concrete")
        {
            if (play)
            {
                randomNumber = Random.Range(0, 4);
                sound = concreteFootSteps.footStepSounds[randomNumber];
                play = false;
                StartCoroutine(Delay(sound, delayAmount)); // Play footstep sound with adjustable delay
            }
        }
    }
    
    private IEnumerator Delay(AudioClip sound, float delay)
    {
        audio_Source.volume = 0.5f;
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
                //("Play Grass Jump");
                sound = grassFootSteps.jumpSound;
                audio_Source.PlayOneShot(sound);
            }

            else if (current == "Pebbles_B_TerrainLayer")
            {
                //Debug.Log("Play Pebble Jump");
                sound = gravelFootSteps.jumpSound;
                audio_Source.PlayOneShot(sound);
                
            }
            else if(current == "Wood" || current == "OutsideWood")
            {
                Debug.Log("Play Wood Jump");
                sound = woodFootSteps.jumpSound;
                audio_Source.PlayOneShot(sound);
            }
            else if(current == "Tile")
            {
                //Debug.Log("Play Tile Jump");
                sound = tileFootSteps.jumpSound;
                audio_Source.PlayOneShot(sound);
            }
            else if(current == "Concrete")
            {
                Debug.Log("Play Concrete Jump");
                sound = concreteFootSteps.jumpSound;
                audio_Source.PlayOneShot(sound);
            }
            jump = false;
        }
       
    }

    private void playLandSound(string current)
    {
        if (current == "Terrain_Layer4_Grass_Plants")
        {
            //Debug.Log("Play Grass Land");
            sound = grassFootSteps.landSound; // assign what sound to play here 
            audio_Source.PlayOneShot(sound);
        }

        else if (current == "Pebbles_B_TerrainLayer")
         {
             //Debug.Log("Play Pebble Land");
             sound = gravelFootSteps.landSound;
             audio_Source.PlayOneShot(sound);
        }
       else if(current == "Wood" || current == "OutsideWood")
        {
            Debug.Log("Play Wood Land");
            sound = woodFootSteps.landSound;
            audio_Source.PlayOneShot(sound);
        }
        else if(current == "Tile")
        {
            //Debug.Log("Play Tile Land");
            sound = tileFootSteps.landSound;
            audio_Source.PlayOneShot(sound);
        }
        else if(current == "Concrete")
        {
            //Debug.Log("Play Tile Land");
            sound = concreteFootSteps.landSound;
            audio_Source.PlayOneShot(sound);
        }
    }

    
}