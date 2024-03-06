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
    [SerializeField] LayerMask playerLayer; // We put player on its own layer becuase when we look for where the palyer is 
    [SerializeField] LayerMask objectLayer;
    [SerializeField] LayerMask groundLayer;
    
    //Patroling
    Vector3 destinationPoint; // where the enemy will be walking to 
    bool WalkPointSet;// this tells if the nemy already has a destination its walking towards or not 
    [SerializeField] float range;// how far the enemy will be allowed to walk 

    protected override void OnInit(BehaviorTree behaviorTree)
    {
        // since our behavior tree is on our enemy game object we can just call what we have below 
        enemy = behaviorTree.gameObject;
        // Implementation below is not necessary 
        //enemy =  GameObject.FindGameObjectWithTag("Enemy");
        
        // can not use getComponent becase node is a scriptable object 
        agent = behaviorTree.GetComponent<NavMeshAgent>(); 
    }
    
    protected override BehaviorTreeNodeResult Evaluate(BehaviorTree behaviorTree)
    {
        patrol();
        return 0;
    }
    
    private void patrol()
    {
        if(!WalkPointSet); // Enemy does not have a point it wants to be walking to 
        { 
            searchForDestination();
        }
        if(WalkPointSet) // the enemy has a location it wants to be navigating to 
        {
            agent.SetDestination(destinationPoint); // 

        }
        if(Vector3.Distance(enemy.transform.position,destinationPoint) < 10)
        {
            WalkPointSet = false;
        }
    }

    private void searchForDestination()
    {
        float newX = Random.Range(-range,range);
        float newZ = Random.Range(-range,range);
        
        // Currently this my cuase problems becuase y is static, so what will happen when the enemy needs to go up stairs? 
        destinationPoint = new Vector3(enemy.transform.position.x+newX, enemy.transform.position.y, enemy.transform.position.z + newZ); 

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
