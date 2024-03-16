using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenBehaviorTrees;
//1. System.Reflection is required for FieldInfo and BindingFlags.
using System.Reflection;
using UnityEngine.AI;

public class Attack : TaskNode
{
    GameObject player;
    //public FadeSound fadeSoundFunction;

    public Animator anim;
    NavMeshAgent agent; // this variable is used to controll navigation of our enemyGameobjet
    public float lastAttackTimestamp;
    public float attackCooldown;

   protected override void OnInit(BehaviorTree behaviorTree)
    {
        //The behaviorTree is on the NPC/ enemy gameobject. We look on that gameobject
        //for the detectors
        // Find the player GameObject by tag
        player = GameObject.FindGameObjectWithTag("Player");
        //fadeSoundFunction = player.GetComponent<FadeSound>();
        anim = behaviorTree.GetComponentInChildren<Animator>();
        agent = behaviorTree.GetComponent<NavMeshAgent>(); 

    }
    protected override BehaviorTreeNodeResult Evaluate(BehaviorTree behaviorTree)
    {
        // Current Time - Last Tiem monster Attached  > Attack Cooldown 
        if(Time.time - lastAttackTimestamp > attackCooldown)
        {
            Debug.Log("Monster Attack Pass");
            anim.Play("Monster Attack");
            float initialSpeed = agent.speed;
            //Debug.Log("Speed Before Attack: "+initialSpeed);
            behaviorTree.StartCoroutine(WhileAttacking());
            agent.speed = initialSpeed;
            //Debug.Log("Speed After Attack: "+agent.speed);
            lastAttackTimestamp = Time.time;
            return BehaviorTreeNodeResult.success;
        }
        else
        {
            Debug.Log("Monster Attack failure");
        }
        return BehaviorTreeNodeResult.failure;
    }

    private IEnumerator WhileAttacking()
    {
        Debug.Log("Inside Attack Function");
        
        // Wait for the next frame to ensure the Animator state is updated
        yield return null;
        
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        while(stateInfo.IsName("Monster Attack"))
        {   
            agent.speed = 0f;
            Debug.Log("Speed While Attacking: " + agent.speed);
            
            // Wait for the next frame to allow the Animator state to update
            yield return null;
            
            // Update the stateInfo for the next iteration
            stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        }

    }

    public override BehaviorTreeNode Clone()
    {
        Attack clonedNode = CreateInstance<Attack>();

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
