using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenBehaviorTrees;
using System.Reflection;

public class DebugNode : TaskNode
{
    public string debugMessage;
    public enum DebugType {Info, Warning, Error }
    public DebugType debugType;

    protected override BehaviorTreeNodeResult Evaluate(BehaviorTree behaviorTree)
    {
        switch (debugType)
        {
            case DebugType.Info:
                Debug.Log(debugMessage);
                break;
            case DebugType.Warning:
                Debug.LogWarning(debugMessage);
                break;
            case DebugType.Error:
                Debug.LogError(debugMessage);
                break;
             default:
                Debug.Log(debugMessage);
                break;
        }
        return BehaviorTreeNodeResult.success;
    }

    public override BehaviorTreeNode Clone()
    {
        DebugNode clonedNode = CreateInstance<DebugNode>();

        // Use reflection to get all fields from the GetEnemyNode Type
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
