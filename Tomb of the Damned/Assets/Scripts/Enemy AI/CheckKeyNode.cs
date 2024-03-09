using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenBehaviorTrees;
using System.Reflection;

public class CheckKeyNode : ConditionNode
{
    public string keyName;

    protected override BehaviorTreeNodeResult Evaluate(BehaviorTree behaviorTree)
    {
        if (behaviorTree.blackboard.TryGetValue(keyName, out object ob))
        {
            if (ob != null && ob is GameObject)
            {
                Debug.Log("Everything Passes");
                return BehaviorTreeNodeResult.success;
            }
        }
        
        return BehaviorTreeNodeResult.failure;
    }


    public override BehaviorTreeNode Clone()
    {
        CheckKeyNode clonedNode = CreateInstance<CheckKeyNode>();

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
