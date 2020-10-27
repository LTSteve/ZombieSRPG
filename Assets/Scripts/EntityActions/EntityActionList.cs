using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityActionList : IEntityAction
{
    private List<IEntityAction> actions;
    public EntityActionList()
    {
        actions = new List<IEntityAction>();
    }

    public void AddAction(IEntityAction action)
    {
        actions.Add(action);
    }

    public void Abort()
    {
        foreach(var action in actions)
        {
            action.Abort();
        }
    }

    public bool IsDone()
    {
        return actions.Count == 0;
    }

    public void Update()
    {
        if(actions.Count == 0)
        {
            return;
        }

        actions[0].Update();
        if (actions[0].IsDone())
        {
            actions.RemoveAt(0);
        }
    }
}