using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TargetingEffect : MonoBehaviour
{
    [SerializeField]
    private Rig headAimRig = null;
    [SerializeField]
    private Rig bodyAimRig = null;
    [SerializeField]
    private Rig weaponAimRig = null;
    [SerializeField]
    private Rig weaponIdleRig = null;
    [SerializeField]
    private Vector3 lookAtOffset = Vector3.zero;

    private Transform target;

    private LerpedFloat aim = new LerpedFloat(1f, 0f);
    private LerpedVector targetPosition = new LerpedVector(1f, Vector3.zero);

    private Vector3 defaultLocalPosition;
    private bool isLocked = false;
    private bool aimingWeapon = false;

    private void Start()
    {
        defaultLocalPosition = transform.localPosition;
    }

    public void LockTarget(Transform toTarget, bool aimWeapon = false)
    {
        target = toTarget;

        targetPosition.SetValue(target.position);
        aim.SetValue(1f);

        isLocked = true;
        aimingWeapon = aimWeapon;
    }

    public void UnlockTarget()
    {
        target = null;

        targetPosition.SetValue(transform.parent.TransformPoint(defaultLocalPosition));
        aim.SetValue(0f);

        isLocked = false;
        aimingWeapon = false;
    }

    private void Update()
    {
        if (target != null)
            //keep track of target
            targetPosition.SetValue(target.position);

        if (headAimRig != null)
            headAimRig.weight = Mathf.Clamp01(aim.GetValue(Time.deltaTime));
        if (bodyAimRig != null)
            bodyAimRig.weight = Mathf.Clamp01(aim.GetValue(Time.deltaTime));
        if (weaponAimRig != null)
        {
            if (aimingWeapon)
                weaponAimRig.weight = Mathf.Clamp01(aim.GetValue(Time.deltaTime));
            else
                weaponAimRig.weight = Mathf.Clamp01(weaponAimRig.weight - Time.deltaTime);
        }
        if (weaponIdleRig != null)
        {
            if (aimingWeapon)
                weaponIdleRig.weight = Mathf.Clamp01(1f - aim.GetValue(Time.deltaTime));
            else
                weaponIdleRig.weight = Mathf.Clamp01(weaponIdleRig.weight + Time.deltaTime);
        }

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
}
