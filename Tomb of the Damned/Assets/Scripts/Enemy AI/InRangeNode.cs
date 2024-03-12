using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenBehaviorTrees;
//1. System.Reflection is required for FieldInfo and BindingFlags.
using System.Reflection;

public class InRangeNode : ConditionNode
{
    public FadeSound fadeSoundFunction;
    GameObject player;
    public enum RangeType {chaseRange, attackRange}
    private AudioSource audioSource; 

    public AudioClip playerBreathing;  
    public RangeType rangeType;
    public NPCRangeDetectors detectors;

    public bool inAttackRange;
    public bool inChaseRange;

    //Gets ran when the node first ticks. Cache a reference to the detectors
    protected override void OnInit(BehaviorTree behaviorTree)
    {
        //The behaviorTree is on the NPC/ enemy gameobject. We look on that gameobject
        //for the detectors
        detectors = behaviorTree.gameObject.GetComponent<NPCRangeDetectors>();

        // Find the player GameObject by tag
        player = GameObject.FindGameObjectWithTag("Player");

    if (player != null)
    {
        // Get the AudioSource component from the player GameObject
        audioSource = player.transform.Find("Audio Source Near Monster").GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.clip = playerBreathing;
        }
        else
        {
            Debug.LogError("Audio Source Near Monster not found on Player GameObject!");
        }
    }
    else
    {
        Debug.LogError("Player GameObject not found!");
    }
    }

    //2. The Evaluate function is where behavior is ran. It is ran every tick.
    protected override BehaviorTreeNodeResult Evaluate(BehaviorTree behaviorTree)
    {
        if(detectors== null)
        {
            return BehaviorTreeNodeResult.failure;
        }

        this.inChaseRange = detectors.inChaseRange;
        this.inAttackRange = detectors.inAttackRange;

        if (this.rangeType == RangeType.attackRange)
        {
            if(detectors.inAttackRange == true)
            {                 
                 return BehaviorTreeNodeResult.success;           
            }
            else
            {
                 audioSource.Play();
                 //Debug.Log("Player Removed");
                 behaviorTree.blackboard.Remove("Player");
                 return BehaviorTreeNodeResult.failure;
            }
    
        }
        
        else
        {
            if(detectors.inChaseRange == true)
            {
                 return BehaviorTreeNodeResult.success;           
            }
            else
            {
                 audioSource.Play();
                 //Debug.Log("Player Removed");
                 behaviorTree.blackboard.Remove("Player");
                 return BehaviorTreeNodeResult.failure;
            }
        }
        
    }

    //3. You must always override the Clone() method. This example function should work in 99% of cases.
    public override BehaviorTreeNode Clone()
    {
        InRangeNode clonedNode = CreateInstance<InRangeNode>();

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
