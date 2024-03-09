using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenBehaviorTrees;
using System.Reflection;

public class GetEnemyNode : TaskNode
{
    // Start is called before the first frame update
    protected override BehaviorTreeNodeResult Evaluate(BehaviorTree behaviorTree)
    {
        //Bail out early if we already have a reference, to avoid having to search for the player again.
        if(HasPlayerReference(behaviorTree))
        {
            return BehaviorTreeNodeResult.success;
        }

        //Find the player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //If it is null for some reason, we failed to cache the player
        if(player == null)
        {
            return BehaviorTreeNodeResult.failure;
        }

        //Add player to blackboard
        behaviorTree.blackboard.SetOrAdd("Player", player);
        Debug.Log("Added Player to BlackBoared");
        return BehaviorTreeNodeResult.success;
    }

    /// <summary>
    /// If we already have a reference to the player in the blackboard, and 
    /// the reference IS a GameObject, we do not need to cache it again. 
    /// </summary>
    private bool HasPlayerReference(BehaviorTree behaviorTree)
    {
   
        if (behaviorTree.blackboard.TryGetValue("Player", out object ob))
        {
            if (ob != null && ob is GameObject)
            {
                return true;
            }
        }

        return false;
    }

    public override BehaviorTreeNode Clone()
    {
        //Creates a runtime copy of the node asset.
        GetEnemyNode clonedNode = CreateInstance<GetEnemyNode>();

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
