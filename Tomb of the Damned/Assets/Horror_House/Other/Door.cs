using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    bool trig, open;
    public float smooth = 2.0f;
    public float DoorOpenAngle = 90.0f;
    private Vector3 defaulRot;
    private Vector3 openRot;
    public Text txt;
    public AudioClip doorOpenSound1;
    public AudioClip doorOpenSound2;
    public AudioClip doorOpenSound3;
    public AudioClip doorCloseSound1;
    public AudioClip doorCloseSound2;
    int randomDoorSound;// will generate random number betwee 1 and 3
    public float doorCooldown = 1.0f; // Add a cooldown period
    private float lastActionTime;

    void Start()
    {
        defaulRot = transform.eulerAngles;
        openRot = new Vector3(defaulRot.x, defaulRot.y + DoorOpenAngle, defaulRot.z);
        lastActionTime = -doorCooldown; // Initialize last action time to allow immediate action
    }

    void Update()
    {
        if (open)
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot, Time.deltaTime * smooth);
        }
        else
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaulRot, Time.deltaTime * smooth);
        }

        if (Input.GetKeyDown(KeyCode.E) && trig && Time.time - lastActionTime > doorCooldown)
        {
            lastActionTime = Time.time;
            open = !open;
            if (open)
            {
                randomDoorSound = Random.Range(1,4);
                if(randomDoorSound == 1)
                    AudioSource.PlayClipAtPoint(doorOpenSound1, transform.position);
                
                else if((randomDoorSound == 2))  
                    AudioSource.PlayClipAtPoint(doorOpenSound2, transform.position);

                else 
                    AudioSource.PlayClipAtPoint(doorOpenSound3, transform.position);
                
                
                
                Debug.Log("Play Door Open Sound");
            }
            else
            {   
                randomDoorSound = Random.Range(1,4);
                if(randomDoorSound == 1)
                    AudioSource.PlayClipAtPoint(doorCloseSound1, transform.position);
                else    
                    AudioSource.PlayClipAtPoint(doorCloseSound2, transform.position);

                Debug.Log("Play Door Close Sound");
            }
        }

        if (trig)
        {
            if (open)
            {
                txt.text = "Close E";
            }
            else
            {
                txt.text = "Open E";
            }
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            if (!open)
            {
                txt.text = "Close E ";
            }
            else
            {
                txt.text = "Open E";
            }
            trig = true;
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Player")
        {
            txt.text = " ";
            trig = false;
        }
    }
}
