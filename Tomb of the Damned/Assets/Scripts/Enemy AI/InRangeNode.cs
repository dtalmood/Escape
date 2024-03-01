using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenBehaviorTrees;
//1. System.Reflection is required for FieldInfo and BindingFlags.
using System.Reflection;

public class InRangeNode : ConditionNode
{
    public enum RangeType {chaseRange, attackRange}

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
            return detectors.inAttackRange == true ? BehaviorTreeNodeResult.success : BehaviorTreeNodeResult.failure;
        }
        else
        {
            return detectors.inChaseRange == true ? BehaviorTreeNodeResult.success : BehaviorTreeNodeResult.failure;
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
