using UnityEngine;
using UnityEngine.AI;

public class LookAtAction : IEntityAction
{
    private TargetingEffect target;

    private bool validSetup;

    public LookAtAction(Transform toLookAt, TargetingEffect target)
    {
        this.target = target;

        validSetup = toLookAt != null && target != null;

        if (validSetup)
        {
            target.LockTarget(toLookAt);
        }
    }

    public void Abort()
    {
        target?.UnlockTarget();
    }

    public bool IsDone()
    {
        return false;
    }

    public void Update()
    {
    }
}