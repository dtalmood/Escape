using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenBehaviorTrees;
using System.Reflection;
using UnityEngine.AI;
using System.Text;
using Unity.VisualScripting;

/*
    Every 30 second the monster respawns if the player has not seen it 
*/

public class MonsterRespawn : TaskNode
{

    public Transform enemyPosition;
    public Transform playerPosition;
    float distanceFromMonsterToPlayer = 20f;

    protected override void OnInit(BehaviorTree behaviorTree)
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        enemyPosition = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    protected override BehaviorTreeNodeResult Evaluate(BehaviorTree behaviorTree)
    {
        Debug.Log("Monster Respawn Function Running");
        bool distanceResult = checkDistance(behaviorTree);

        // We check if the player is far enough away from the monster to justify the monster to respawn 
        if (distanceResult) // monster is far enough away
        {
            Debug.Log("Player is far enough away");
            // check and make sure the player is not looking at the monster during the respawn
            
            if (!checkVision(behaviorTree)) // player is looking at the monster so do not respawn 
            {
                return BehaviorTreeNodeResult.failure; 
            }
            
            Debug.Log("Player is not looking at the monster");
            respawnMonster();
            return BehaviorTreeNodeResult.success;
        }
        else // monster is still close 
        {
            Debug.Log("Monster is too close to intiate a respawn");
            return BehaviorTreeNodeResult.failure;
        }
    }
    private void respawnMonster()
    {
        Debug.Log("Rewspawn the monster now");
        // Calculate the direction vector from the monster to the player
        Vector3 directionToPlayer = (playerPosition.position - enemyPosition.position).normalized;

        // Calculate the new position for the monster slightly closer to the player
        Vector3 newPosition = enemyPosition.position + directionToPlayer * distanceFromMonsterToPlayer;

        // Teleport the monster to the new position
        enemyPosition.position = newPosition;
    }


    // if distance of the mosnter is large enough to just a respawn it will return True 
    private bool checkDistance(BehaviorTree behaviorTree)
    {
        float val = Vector3.Distance(enemyPosition.position, playerPosition.position);
        int distance = (int)val;

        Debug.Log("Distance = "+ distance);
        if(distance > 20)
            return true; 
        
        return false;
    }

    private bool checkVision(BehaviorTree behaviorTree)
    {
        if (playerPosition)
        {
            Vector3 forward = playerPosition.transform.TransformDirection(Vector3.forward);
            Vector3 toOther = enemyPosition.position - playerPosition.transform.position;

            if (Vector3.Dot(forward, toOther) < 0)
            {
                // Debug.Log("Player Removed");
                behaviorTree.blackboard.Remove("Player");
                Debug.Log("The Monster is behind me!");
                // behaviorTree.blackboard.
                return true;
            }
            else if (Vector3.Dot(forward, toOther) > 0)
            {
                Debug.Log("The monster is in front me!");
                return false;
            }
        }
        else
        {
            Debug.Log("Player position not found");
            return false;
        }
        return false; // Default return value to handle all cases
    }

    public override BehaviorTreeNode Clone()
    {
        MonsterRespawn clonedNode = CreateInstance<MonsterRespawn>();

        // Use reflection to get all fields
        FieldInfo[] fields = this.GetType().GetFields(
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance
        );

        foreach (FieldInfo field in fields)
        {
            // Copy the value of each field from the original to the cloned object
            field.SetValue(clonedNode, field.GetValue(this));
        }

        return clonedNode;
    }
}
