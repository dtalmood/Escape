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

    public GameObject enemy;
    public GameObject player;
    // float distanceFromMonsterToPlayer = 1f;

    protected override void OnInit(BehaviorTree behaviorTree)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    protected override BehaviorTreeNodeResult Evaluate(BehaviorTree behaviorTree)
    {
        //Debug.Log("Player Position: " + player.transform.position + ", Monster Position: " + enemy.transform.position);

        bool distanceResult = checkDistance(behaviorTree);

        // We check if the player is far enough away from the monster to justify the monster to respawn 
        if (distanceResult) // monster is far enough away
        {
            // check and make sure the player is not looking at the monster during the respawn
            
            if (!checkVision(behaviorTree)) // player is looking at the monster so do not respawn 
            {
                return BehaviorTreeNodeResult.failure; 
            }
            
            respawnMonster();
            return BehaviorTreeNodeResult.success;
        }
        else // monster is still close 
        {
            //Debug.Log("Monster is too close to intiate a respawn");
            return BehaviorTreeNodeResult.failure;
        }
    }
    private void respawnMonster()
    {
        Debug.Log("Respawn monster now");
        // Calculate the direction vector from the monster to the player
        Vector3 directionToPlayer = (player.transform.position - enemy.transform.position).normalized;

        // Calculate the new position for the monster slightly closer to the player
        // Vector3 newPosition = enemy.transform.position + directionToPlayer * distanceFromMonsterToPlayer;
        Vector3 bufferVector = new Vector3(5, 0, 5);
        Vector3 newPosition = player.transform.position + directionToPlayer + bufferVector;

        // Teleport the monster to the new position
        enemy.transform.position = newPosition;
    }


    // if distance of the mosnter is large enough to just a respawn it will return True 
    private bool checkDistance(BehaviorTree behaviorTree)
    {
        float val = Vector3.Distance(enemy.transform.position, player.transform.position);
        int distance = (int)val;

        //Debug.Log("Distance = "+ distance);
        if(distance > 20)
            return true; 
        
        return false;
    }

    private bool checkVision(BehaviorTree behaviorTree)
    {
        if (player.transform)
        {
            Vector3 forward = player.transform.TransformDirection(Vector3.forward);
            Vector3 toOther = enemy.transform.position - player.transform.position;

            //Debug.Log("Vector 3 Dot: "+Vector3.Dot(forward, toOther));
            /*
                How the Vector3.Dot(forward, toOther) Works 
                    1.  Player Looking directly at monster, Output: 1 
                    2. Player looks a +/- 10 degrees left or right of monstet, Output: 0.9
                    3. Player looks a +/- 20 degrees left or right of monstet, Output: 0.8
                    4. Player looks a +/- 30 degrees left or right of monstet, Output: 0.7
                    5. Player looks a +/- 40 degrees left or right of monstet, Output: 0.6
                    6. Player looks a +/- 50 degrees left or right of monstet, Output: 0.5
                    7. Player looks a +/- 60 degrees left or right of monstet, Output: 0.4
                    7. Player looks a +/- 70 degrees left or right of monstet, Output: 0.3 
            */
            if (Vector3.Dot(forward, toOther) < 0.3)
            {
                // Debug.Log("Player Removed");
                behaviorTree.blackboard.Remove("Player");
                //Debug.Log("The Monster is behind me!");
                // behaviorTree.blackboard.
                return true;
            }
            else 
            {
                //Debug.Log("The monster is in front me!");
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
