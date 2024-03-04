using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenBehaviorTrees;
using System.Reflection;

public class ConeCheck : ConditionNode
{
    public Transform enemyPosition;
    public Transform playerPosition;
    protected override void OnInit(BehaviorTree behaviorTree)
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyPosition = GameObject.FindGameObjectWithTag("EnemyPosition").GetComponent<Transform>();
    }

    protected override BehaviorTreeNodeResult Evaluate(BehaviorTree behaviorTree)
    {
        if (enemyPosition)
        {
            Vector3 forward = enemyPosition.transform.TransformDirection(Vector3.forward);
            Vector3 toOther = playerPosition.position - enemyPosition.transform.position;


            if (Vector3.Dot(forward, toOther) < 0)
            {
                Debug.Log("The other transform is behind me!");
                return BehaviorTreeNodeResult.failure;
            }
            else
            {
                Debug.Log("The other transform is in front me!");
                return BehaviorTreeNodeResult.success;
            }
        }
        else
        {
            Debug.Log("Public Objects not set to this instance");
            return BehaviorTreeNodeResult.failure;
        }

    }
    public override BehaviorTreeNode Clone()
    {
        ConeCheck clonedNode = CreateInstance<ConeCheck>();

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
