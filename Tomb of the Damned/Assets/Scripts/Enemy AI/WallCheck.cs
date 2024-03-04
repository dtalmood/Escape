using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenBehaviorTrees;
//1. System.Reflection is required for FieldInfo and BindingFlags.
using System.Reflection;

public class WallCheck : ConditionNode
{
    public Transform playerTransform;
    public Transform enemnyTransform;
    // Start is called before the first frame update
    protected override void OnInit(BehaviorTree behaviorTree)
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemnyTransform = GameObject.FindGameObjectWithTag("EnemyPosition").GetComponent<Transform>();
        if(playerTransform == null)
        {
            Debug.Log("Player Not FOund");
        }
       else if(enemnyTransform == null)
        {
            Debug.Log("Enemy not Found");
        }

    }
    protected override BehaviorTreeNodeResult Evaluate(BehaviorTree behaviorTree)
    {
        Ray ray = new Ray(enemnyTransform.position, (playerTransform.position - enemnyTransform.position).normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            // Check if the hit object has the "Wall" tag
            if (hit.collider.CompareTag("Wall"))
            {
                Debug.Log("Wall in front of the player");
                return BehaviorTreeNodeResult.success;
            }
            
            else
            {
                // Output the tag of the object hit (for debugging purposes)
                Debug.Log("Tag Hit: " + hit.collider.tag);

                // Return failure for any other tags
                Debug.Log("No Wall");
                return BehaviorTreeNodeResult.failure;
            }
        }
        else
        {
            Debug.Log("No ray traced path found");
            return BehaviorTreeNodeResult.failure;
        }
    }
    

    public override BehaviorTreeNode Clone()
    {
        WallCheck clonedNode = CreateInstance<WallCheck>();

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

    // Update is called once per frame
   
}
