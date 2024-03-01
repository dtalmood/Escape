using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenBehaviorTrees;
using System.Reflection;
using Unity.VisualScripting;

public class Wall : ConditionNode
{
    public WallDetector detector;
    
    protected override void OnInit(BehaviorTree behaviorTree)
    {
        //The behaviorTree is on the wall Detector gameobject. We look on that gameobject
        //for the detectors
        detector = behaviorTree.gameObject.GetComponent<WallDetector>();
    }
    
    This.
    
    protected override BehaviorTreeNodeResult Evaluate(BehaviorTree behaviorTree)
    {
        if(detector == null)
        {
            return BehaviorTreeNodeResult.failure;
        }

        if(detector.checkWall == false)
        {
            return BehaviorTreeNodeResult.failure;
        }
        else
        
            return BehaviorTreeNodeResult.success;
        }
    }

    public override BehaviorTreeNode Clone()
    {
        Wall clonedNode = CreateInstance<Wall>();

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
