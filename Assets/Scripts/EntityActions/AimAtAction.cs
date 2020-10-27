using UnityEngine;
using UnityEngine.AI;

public class AimAtAction : IEntityAction
{
    private TargetingEffect target;
    private Transform targeter;
    private float aimRepositionAngle;
    private ThirdPersonMover mover;

    private bool validSetup;

    private Vector3 repositionTo = Vector3.zero;

    public AimAtAction(Transform toLookAt, IEntity targeter, bool aimWeapon = false)
    {
        this.targeter = targeter?.GetTransform();
        aimRepositionAngle = targeter == null ? -1 : targeter.GetAimRepositionAngle();
        target = targeter?.GetTargeting();
        mover = targeter?.GetMover();

        validSetup = toLookAt != null && targeter != null && target != null && this.targeter != null && mover != null;

        if (validSetup)
        {
            target.LockTarget(toLookAt, aimWeapon);
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

        _checkLookAngle();
        _rotateToTarget();
    }

    private void _checkLookAngle()
    {
        if (aimRepositionAngle == -1) return;

        //zero out y movement
        var targetPosition = new Vector3(target.transform.position.x, targeter.position.y, target.transform.position.z);

        //get relative target direction
        var targetRelativeDirection = (targetPosition - targeter.position).normalized;
        //get forward direction
        var forwardDirection = targeter.forward;
        //calculate the targetingAngle
        var targetingAngle = Vector3.Angle(forwardDirection, targetRelativeDirection);

        if(Mathf.Abs(targetingAngle) > aimRepositionAngle)
        {
            repositionTo = targetRelativeDirection;
        }
        if(Mathf.Abs(Vector3.Angle(repositionTo, forwardDirection)) < 10f) //close enough
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