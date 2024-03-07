using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenBehaviorTrees;
using System.Reflection;
using UnityEngine.AI;
using System.Text;
using Unity.VisualScripting;

// this function server only to get the live speed of the monster inorder to correcrlt play the monster walking and running animation
public class MonsterSpeed : TaskNode
{
    private Animator animator;
     NavMeshAgent agent;
     
    protected override void OnInit(BehaviorTree behaviorTree)
    {
        agent = behaviorTree.GetComponent<NavMeshAgent>(); 
        animator = behaviorTree.GetComponentInChildren<Animator>();
    }

    protected override BehaviorTreeNodeResult Evaluate(BehaviorTree behaviorTree)
    {
        Debug.Log("Velocity = "+ agent.velocity.magnitude);
         if( agent.velocity.magnitude > 0.01f)
            animator?.SetBool("Idle",false);
        else
            animator?.SetBool("Idle",true);
        return BehaviorTreeNodeResult.success;
    }

    public override BehaviorTreeNode Clone()
    {
        MonsterSpeed clonedNode = CreateInstance<MonsterSpeed>();

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
