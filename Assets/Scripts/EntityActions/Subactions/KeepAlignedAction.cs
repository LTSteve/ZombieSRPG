using UnityEngine;
using UnityEngine.AI;

public class KeepAlignedAction : IEntityAction
{
    //manage rotation
    private Transform transformToKeepAligned;
    private Transform targetOfAlignment;
    private ThirdPersonMover mover;
    private float aimRepositionAngle;
    private float aimOuterLimitAngle;

    private Vector3 repositionTo = Vector3.zero;
    private float targetingAngle = 0f;

    private bool validSetup;

    public KeepAlignedAction(IEntity entityToKeepAligned, Transform target)
    {
        transformToKeepAligned = entityToKeepAligned?.GetTransform();
        mover = entityToKeepAligned?.GetMover();
        targetOfAlignment = target;

        aimRepositionAngle = entityToKeepAligned == null ? -1 : entityToKeepAligned.GetAimRepositionAngle();
        aimOuterLimitAngle = entityToKeepAligned == null ? -1 : entityToKeepAligned.GetAimAngleLimit();

        validSetup = transformToKeepAligned != null && targetOfAlignment != null && mover != null;
    }

    public void Abort()
    {

    }

    public bool IsDone()
    {
        return !validSetup ||
            transformToKeepAligned == null || transformToKeepAligned.gameObject == null ||
            targetOfAlignment == null || targetOfAlignment.gameObject == null;
    }

    public bool IsWithinOuterLimit()
    {
        return targetingAngle < aimOuterLimitAngle;
    }

    public void Update()
    {
        if (IsDone()) return;

        _checkLookAngle();
        _rotateToTarget();
    }

    private void _checkLookAngle()
    {
        if (aimRepositionAngle == -1) return;

        //zero out y movement
        var targetPosition = new Vector3(targetOfAlignment.transform.position.x, transformToKeepAligned.position.y, targetOfAlignment.transform.position.z);

        //get relative target direction
        var targetRelativeDirection = (targetPosition - transformToKeepAligned.position).normalized;
        //get forward direction
        var forwardDirection = transformToKeepAligned.forward;
        //calculate the targetingAngle
        targetingAngle = Vector3.Angle(forwardDirection, targetRelativeDirection);

        if (targetingAngle > aimRepositionAngle)
        {
            repositionTo = targetRelativeDirection;
        }
        if (Vector3.Angle(repositionTo, forwardDirection) < 10f) //close enough
        {
            repositionTo = Vector3.zero;
        }
    }

    private void _rotateToTarget()
    {
        //turn towards target
        mover.AlignTo(repositionTo);
    }
}