using UnityEngine;
using UnityEngine.AI;

public class AimAtAction : IEntityAction
{
    //maintain reference to target
    private TargetingEffect target;

    //manage rotation
    private KeepAlignedAction keepAligned;

    private bool validSetup;

    public AimAtAction(Transform toLookAt, IEntity targeter)
    {
        target = targeter?.GetTargeting();

        if(target != null)
            keepAligned = new KeepAlignedAction(targeter, toLookAt);

        validSetup = toLookAt != null && target != null && keepAligned != null;

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
        if (!validSetup) return;

        keepAligned.Update();
    }
}