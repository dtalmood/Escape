using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenBehaviorTrees;
using System.Reflection;
using UnityEngine.AI;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;

public class ChasePlayer : ConditionNode
{
    GameObject player;
    GameObject enemy;
    NavMeshAgent agent; // this variable is used to controll navigation of our enemyGameobjet
    //[SerializeField] LayerMask playerLayer; // We put player on its own layer becuase when we look for where the palyer is 
    [SerializeField] LayerMask objectLayer;
    [SerializeField] LayerMask groundLayer;
    public AudioClip chaseMusic; 
    public FadeSound fadeSoundFunction;
    
    [SerializeField] 
    [ReadOnly]   
    private string initalPositionDictionaryKey = "InitalRoamPosition";
    
    public float speed = 2f;

    //Patroling
    Vector3 destinationPoint; // where the enemy will be walking to 
    bool WalkPointSet;// this tells if the nemy already has a destination its walking towards or not 
    [SerializeField] float range;// how far the enemy will be allowed to walk 

    private Animator animator;
    protected override void OnInit(BehaviorTree behaviorTree)
    {
        
        player = GameObject.FindGameObjectWithTag("Player");

        // since our behavior tree is on our enemy game object we can just call what we have below 
        enemy = behaviorTree.gameObject;
        // Implementation below is not necessary 
        //enemy =  GameObject.FindGameObjectWithTag("Enemy");

        // can not use getComponent becase node is a scriptable object 
        agent = behaviorTree.GetComponent<NavMeshAgent>(); 

        // The blackboard dictionary has (string, object) pairs. We are adding a new entry
        //with the string key equal to our initialPosition string
        //and the value is the initial position of the game object (The NPC)
        animator = behaviorTree.GetComponentInChildren<Animator>();
        fadeSoundFunction = player.GetComponent<FadeSound>();
    }
    
    protected override BehaviorTreeNodeResult Evaluate(BehaviorTree behaviorTree)
    {
        agent.speed = 2f;
        Debug.Log("Speed in Chase: "+agent.speed);
        //fadeSoundFunction.fadeInChaseMusic(chaseMusic);
        //Debug.Log("Chase is Running");
        animator?.SetBool("Chase",true);
        agent.SetDestination(GetDestination(behaviorTree.transform.position, player.transform.position));
        return BehaviorTreeNodeResult.success;
    }
    
    public Vector3 GetDestination(Vector3 currentPosition, Vector3 target)
    {
        float distanceFromTarget = 1f;
        Vector3 des = target + (currentPosition - target).normalized * distanceFromTarget;
        return des;
    } 

    public override BehaviorTreeNode Clone()
    {
        ChasePlayer clonedNode = CreateInstance<ChasePlayer>();

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
