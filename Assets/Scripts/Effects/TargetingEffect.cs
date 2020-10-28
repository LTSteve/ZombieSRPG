using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TargetingEffect : MonoBehaviour
{
    [SerializeField]
    private EntityRigStatemachine rigs = null;
    [SerializeField]
    private Vector3 lookAtOffset = Vector3.zero;

    private IEntity parentEntity = null;

    private Transform target;

    private LerpedVector targetPosition = new LerpedVector(0.25f, Vector3.zero);

    private Vector3 defaultLocalPosition;
    private bool isLocked = false;

    private Vector3 currentMovementPrediction = Vector3.zero;
    private Vector3 previousTargetLocation = Vector3.zero;
    private float bulletSpeed = 0f;

    private void Start()
    {
        defaultLocalPosition = transform.localPosition;
        parentEntity = gameObject.GetComponentInParent<IEntity>();
    }

    public void LockTarget(Transform toTarget, bool aimWeapon = false, float bulletSpeed = 0f)
    {
        target = toTarget;
        this.bulletSpeed = bulletSpeed;

        targetPosition.SetValue(target.position);
        rigs.Aiming(aimWeapon);

        isLocked = true;
    }

    public void UnlockTarget()
    {
        target = null;

        targetPosition.SetValue(transform.parent.TransformPoint(defaultLocalPosition));
        rigs.Holding();

        isLocked = false;
    }

    public bool TargetIsDead()
    {
        return target == null || target.gameObject == null;
    }

    private void Update()
    {
        if(TargetIsDead() && isLocked)
        {
            UnlockTarget();
        }

        if (target != null)
            //keep track of target
            targetPosition.SetValue(target.position + _predictTargetMovement());

        if (isLocked)
        {
            transform.position = targetPosition.GetValue(Time.deltaTime) + lookAtOffset;
        }
        else //free to reset aim position to forward
        {
            //keep track of in front of me
            targetPosition.SetValue(transform.parent.TransformPoint(defaultLocalPosition));
            transform.position = targetPosition.GetValue(Time.deltaTime);
        }
    }

    private Vector3 _predictTargetMovement()
    {
        //No tracking needed
        if (bulletSpeed == 0f || parentEntity == null) return Vector3.zero;

        var overshootSeconds = Vector3.Distance(target.position, parentEntity.GetPosition()) / bulletSpeed;

        var nextMovementPrediction = (target.position - previousTargetLocation) * (overshootSeconds / Time.deltaTime);

        currentMovementPrediction = currentMovementPrediction * (0.5f) + nextMovementPrediction * (0.5f);

        previousTargetLocation = target.position;

        return currentMovementPrediction;
    }
}
