using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class NPCRangeDetectors : MonoBehaviour
{
    public PlayerDetector attackDetector;
    public PlayerDetector chaseDetector;

    public bool inChaseRange;
    public bool inAttackRange;

    public void PlayerExitedChaseRange()
    {
        inChaseRange = false;
    }

    public void PlayerEnteredChaseRange()
    {
        inChaseRange = true;
    }

    public void PlayerExitedAttackRange()
    {
        inAttackRange = false;
    }

    public void PlayerEnteredAttackRange()
    {
        inAttackRange = true;
    }

    /*
        1. We want to constnaltly have this Ai to be roaming 
            Condition 1: if over 30 seconds have passed and the enemy has not found player thenmy Respaws  within some X or Y distance 

    */
    void Update()
    {
        
    }
}
