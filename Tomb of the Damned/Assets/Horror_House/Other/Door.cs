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
    public AudioClip[] doorOpenSounds; // Array of door open sounds
    public AudioClip[] doorCloseSounds; // Array of door close sounds
    public float doorCooldown = 1.0f;
    private float lastActionTime;

    void Start()
    {
        defaulRot = transform.eulerAngles;
        openRot = new Vector3(defaulRot.x, defaulRot.y + DoorOpenAngle, defaulRot.z);
        lastActionTime = -doorCooldown;
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
                // Randomly select a door open sound
                int randomIndex = Random.Range(0, doorOpenSounds.Length);
                AudioClip selectedOpenSound = doorOpenSounds[randomIndex];
                AudioSource.PlayClipAtPoint(selectedOpenSound, transform.position);
                //Debug.Log("Play Door Open Sound");
            }
            else
            {
                // Randomly select a door close sound
                int randomIndex = Random.Range(0, doorCloseSounds.Length);
                AudioClip selectedCloseSound = doorCloseSounds[randomIndex];
                AudioSource.PlayClipAtPoint(selectedCloseSound, transform.position);
                //Debug.Log("Play Door Close Sound");
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
