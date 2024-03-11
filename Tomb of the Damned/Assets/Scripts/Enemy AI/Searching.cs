using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenBehaviorTrees;
using System.Reflection;
using UnityEngine.AI;
using System.Text;
using Unity.VisualScripting;

// This Script will hanle the enemy searching for the player
public class Searching : ConditionNode
{
    GameObject enemy;
    NavMeshAgent agent; // this variable is used to controll navigation of our enemyGameobjet
    //[SerializeField] LayerMask playerLayer; // We put player on its own layer becuase when we look for where the palyer is 
    [SerializeField] LayerMask objectLayer;
    [SerializeField] LayerMask groundLayer;
    
    [SerializeField] 
    [ReadOnly]   
    private string initalPositionDictionaryKey = "InitalRoamPosition";
    

    //Patroling
    Vector3 destinationPoint; // where the enemy will be walking to 
    bool WalkPointSet;// this tells if the nemy already has a destination its walking towards or not 
    [SerializeField] float range;// how far the enemy will be allowed to walk 

    private Animator animator;
    protected override void OnInit(BehaviorTree behaviorTree)
    {
        // since our behavior tree is on our enemy game object we can just call what we have below 
        enemy = behaviorTree.gameObject;
        // Implementation below is not necessary 
        //enemy =  GameObject.FindGameObjectWithTag("Enemy");

        // can not use getComponent becase node is a scriptable object 
        agent = behaviorTree.GetComponent<NavMeshAgent>(); 

        // The blackboard dictionary has (string, object) pairs. We are adding a new entry
        //with the string key equal to our initialPosition string
        //and the value is the initial position of the game object (The NPC)
        behaviorTree.blackboard.Add(initalPositionDictionaryKey, behaviorTree.transform.position);
        animator = behaviorTree.GetComponentInChildren<Animator>();
    }
    
    protected override BehaviorTreeNodeResult Evaluate(BehaviorTree behaviorTree)
    {
        animator?.SetBool("Chase",false);
        patrol(behaviorTree);
        return BehaviorTreeNodeResult.success;
    }
    
    private void patrol(BehaviorTree behaviorTree)
    {
        if(!WalkPointSet) // Enemy does not have a point it wants to be walking to 
        {                           
            searchForDestination(behaviorTree);
        }
        if(WalkPointSet) // the enemy has a location it wants to be navigating to 
        {
            //This line makes the NPC move on the navmesh to the given position (destinationPoint)
            agent.SetDestination(destinationPoint);

        }
        if(Vector3.Distance(enemy.transform.position,destinationPoint) < 10)
        {
            WalkPointSet = false;
        }
    }

    private void searchForDestination(BehaviorTree behaviorTree)
    {
        //Set newX and newZ with initial values
        float newX = 0;
        float newZ = 0;


        //Get the initial starting position of the monster from the blackboard (We stored it in OnInit).
        //If we can't get it from the blackboard, just set it equal to current position
        //All blackboard values are stored as object. We need to cast it to Vector3 (what we stored it as)
        Vector3 initialPosVector;
        if(behaviorTree.blackboard.TryGetValue(initalPositionDictionaryKey, out object ob))
        {
            initialPosVector = (Vector3)ob;
        }
        else{
            initialPosVector = behaviorTree.transform.position;
        }
      
        //Initialize inRange to false. Get a random position that is within range of the starting position
        bool inRange = false;
        while(!inRange)
        {
            newX = Random.Range(-range,range);
            newZ = Random.Range(-range,range);
            // Currently this my cuase problems becuase y is static, so what will happen when the enemy needs to go up stairs? 
            destinationPoint = new Vector3(initialPosVector.x+newX, initialPosVector.y, initialPosVector.z + newZ); 
            
            float distance = Vector3.Distance(initialPosVector, destinationPoint);
            inRange = distance < range;
            //Debug.Log($"initial position {initialPosVector}, target position: {destinationPoint}, distance: {distance}");
        }

        // Now we must check whether or not what the random chose is in range of the NevMesh 
        if(Physics.Raycast(destinationPoint,Vector3.down,groundLayer))
            WalkPointSet = true;
        else if(Physics.Raycast(destinationPoint,Vector3.down,objectLayer))
            WalkPointSet = true;    
        else
        {
            WalkPointSet = false;
        }

    }

    public override BehaviorTreeNode Clone()
    {
        Searching clonedNode = CreateInstance<Searching>();

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
