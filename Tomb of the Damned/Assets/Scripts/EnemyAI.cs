using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    public PlayerDetector attackDetector;
    public PlayerDetector chaseDetector;

    public bool inChaseRange;
    public bool inAttackRange;

    // Start is called before the first frame update
    void Start()
    {
        chaseDetector.onPlayerEnter = new UnityEvent();
        chaseDetector.onPlayerEnter.AddListener(PlayerEnteredChaseRange);

        chaseDetector.onPlayerExit = new UnityEvent();
        chaseDetector.onPlayerExit.AddListener(()=>{
            this.inChaseRange = false;
        });

        attackDetector.onPlayerEnter = new UnityEvent();
        attackDetector.onPlayerEnter.AddListener(PlayerEnteredAttackRange);      
        chaseDetector.onPlayerExit.AddListener(()=>{
            this.inAttackRange = false;
        });  
    }

    public void PlayerEnteredChaseRange()
    {
        inChaseRange = true;
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
