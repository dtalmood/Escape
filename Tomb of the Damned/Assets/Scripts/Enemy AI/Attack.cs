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
    PlayerHealthBar playerHealth;
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
        playerHealth = player.GetComponentInChildren<PlayerHealthBar>();

    }
    protected override BehaviorTreeNodeResult Evaluate(BehaviorTree behaviorTree)
    {
        // Current Time - Last Tiem monster Attached  > Attack Cooldown 
        if(Time.time - lastAttackTimestamp > attackCooldown)
        {
            Debug.Log("Monster Attack Pass");
            anim.Play("Monster Attack"); 
            behaviorTree.StartCoroutine(WhileAttacking());
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
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float initialSpeed = agent.speed;
        while(stateInfo.IsName("Monster Attack"))
        {   
            
            agent.speed = 0f;
            Debug.Log("Speed While Attacking: "+ agent.speed);
            yield return null;
            stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        }
        agent.speed = initialSpeed;
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
