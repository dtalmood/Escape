using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class PlayerDetector : MonoBehaviour
{
    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerExit;
    public UnityEvent onPlayerStay;

    public SphereCollider sphereCollider;

    public void Start()
    {
        if(sphereCollider ==  null)
        {
            sphereCollider = GetComponent<SphereCollider>();
        }

        Debug.Log($"Start method for {gameObject.name} called");
    }

    public void Update()
    {
        //Debug.Log($"Update method for {gameObject.name} called");
    }


    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
//            Debug.Log($"Player Staying In {gameObject.name} Collider");
            onPlayerStay?.Invoke();
        }
        else{
            Physics.IgnoreCollision(sphereCollider, other);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Debug.Log($"Player Entered {gameObject.name} Collider");
            onPlayerEnter?.Invoke();
        }
        else{
            Physics.IgnoreCollision(sphereCollider, other);
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Debug.Log($"Player Exited {gameObject.name} Collider");
            onPlayerExit?.Invoke();
        }
        else{
            Physics.IgnoreCollision(sphereCollider, other);
        }

    }
    


}
