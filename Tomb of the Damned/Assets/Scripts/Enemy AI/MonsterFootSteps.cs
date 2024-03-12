using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenBehaviorTrees;
using System.Reflection;
using UnityEngine.AI;
using System.Text;
using Unity.VisualScripting;
using TMPro;

// this function server only to get the live speed of the monster inorder to correcrlt play the monster walking and running animation
public class MonsterFootSteps : MonoBehaviour
{
    public GameObject monster;
    public float monsterHeight = 3f;
     NavMeshAgent agent;
    public TerrainDetector terrainDetector;
     bool groundedObject; // Tells us if we are on object
    bool groundedTerrain; // tells us if we are on Terrain
    private float walkSoundDelay = 0.7f; // how much to delay sound between footsteps when walking 
    private float sprintSoundDelay = 0.6f; // how much to delay sound between footsteps when sprinting
    int randomNumber;
    RaycastHit hit;
    bool play = true;



    [Header("Layers")]
    public LayerMask ObjectGround; // layer used to detrmine if we are on a 3d object 
    public LayerMask TerrainGround;// layer used to detrmine if we are on a Terrain 
    

     [Header("Sound")]
     public string current;
     public AudioClip sound; 
     public footStepCollection grassFootSteps; // this object holds the sand sounds 
    public footStepCollection gravelFootSteps; // this object holds gravel sounds 
    public footStepCollection woodFootSteps; // this object holds the sand sounds 
    public footStepCollection tileFootSteps; // this object holds gravel sounds
    public footStepCollection concreteFootSteps; // this object holds gravel sounds 

     public AudioSource audio_Source; // this Game Object will be used to switch the the correct sound and play the proper foot step
     

    void Start()
    {
        terrainDetector = GetComponent<TerrainDetector>();
        agent = GetComponent<NavMeshAgent>(); 
        audio_Source = transform.Find("Audio Source").GetComponent<AudioSource>();// this will search unity for a component that has specified data inside of it  
        monster = gameObject;
    }
        
    

    void Update()
    {
        groundOrTerrain(); // handles and checks whether the player is on ground or terrain
        playFootStepSound(); // handles playing monste footstep sounds
    }

    private void groundOrTerrain()
    {
        // Checks If Monster is on 3D object 
        groundedObject = Physics.Raycast(monster.transform.position, Vector3.down, monsterHeight * 0.18f + 0.1f, ObjectGround);
        //Debug.Log("Ground: "+ groundedObject);

        // Checks if monster is on terrain
        groundedTerrain = Physics.Raycast(monster.transform.position, Vector3.down, monsterHeight * 0.1f + 0.1f, TerrainGround);
        groundedTerrain = !groundedObject && groundedTerrain; 
        //Debug.Log("Terrain: "+ groundedTerrain);
        

        
    }

   
    private void playFootStepSound()
    {
        if(agent.velocity.magnitude > 0.01f) // monster is moving
        {
            
            if (groundedTerrain) // Monster is walking on Terrain
            {
                current = terrainDetector.getLayerName();
                //Debug.Log("Current = " + current);  
                terrainPlaySound(current, walkSoundDelay);

            }

            else // Monster is walking on 3D object
            {
                if (Physics.Raycast(monster.transform.position, Vector3.down, out hit, monsterHeight * 0.5f + 0.2f, ObjectGround))
                {
                    current  = hit.collider.gameObject.tag;
                    //Debug.Log("Current = " + current);  
                    objectPlaySound(current, walkSoundDelay);

                }
            }
        }
        else // monster is not moving 
        {

        }
        
    }
    public void terrainPlaySound(string current, float delayAmount)
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
    
    public void objectPlaySound(string current, float delayAmount)
    {
        if(current == "Wood")
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

}
