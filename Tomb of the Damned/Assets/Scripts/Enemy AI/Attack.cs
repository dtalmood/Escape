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
        if(Time.time - lastAttackTimestamp > attackCooldown)
        {
            Debug.Log("Attack Successful");
            anim.Play("Monster Attack");
          
            behaviorTree.StartCoroutine(WhileAttacking());

            lastAttackTimestamp = Time.time;
            return BehaviorTreeNodeResult.success;
        }
        else
        {
            Debug.Log("Attack Failure");
        }
        
        return BehaviorTreeNodeResult.failure;
    }

    private IEnumerator WhileAttacking()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float initialSpeed = agent.speed;
        Debug.Log("Previous Agent Speed = "+ agent.speed);
        while(stateInfo.IsName("Monster Attack"))
        {
            
            agent.speed = 0;
            yield return null;
        }
        Debug.Log("Current Agent Speed = "+ agent.speed);
        agent.speed = 2f;
        yield return null;
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
