using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
    public Transform playerPosition;
    // this is the position of the player game object in which the palyer is moving around 

    void Update()
    {
        // if (playerPosition)
        // {
        //     Vector3 forward = transform.TransformDirection(Vector3.forward);
        //     Vector3 toOther = playerPosition.position - transform.position;

        //     if (Vector3.Dot(forward, toOther) < 0)
        //     {
        //         print("The other transform is behind me!");
                
        //     }
        //     else if (Vector3.Dot(forward, toOther) > 0)
        //     {
        //         print("The other transform is in front me!");
        //     }
        // }
    }
}
