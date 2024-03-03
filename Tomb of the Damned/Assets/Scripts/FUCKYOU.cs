using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FUCKYOU : MonoBehaviour
{
    public Transform playerTransform;
    public Transform enemnyTransform;
    public bool playerBehindWall;
    
    // Update is called once per frame
    void Update()
    {
        // Check 1: Player Distance from the enemy 
        float Distance = Vector3.Distance(playerTransform.position, enemnyTransform.position);
        int ceiling = Mathf.CeilToInt(Distance);
        //Debug.Log("Current Distance between Player:"+ ceiling);
        checkWall();

        //Check 2: If player is close enough then we should be checking if there is a wall between the enemy and the Player
        
        
    }

    public void checkWall()
    {
        Ray ray = new Ray(enemnyTransform.position, (playerTransform.position - enemnyTransform.position).normalized);
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.yellow);
            
            if(hit.collider.CompareTag("Wall"))
            {
                //Debug.Log("WEEEE WEEEEE WEEEE WEEEEE WEEEE WEEEEE WEEEE WEEEEE WEEEE WEEEEE WEEEE WEEEEE WEEEE WEEEEE WEEEE WEEEEE ");
                playerBehindWall = false;
            }
            else
            {
                //Debug.Log("POOO POOOO POOO POOOO POOO POOOO POOO POOOO POOO POOOO POOO POOOO POOO POOOO POOO POOOO POOO POOOO POOO POOOO ");
                playerBehindWall = true;

            }
        }

    }
}
