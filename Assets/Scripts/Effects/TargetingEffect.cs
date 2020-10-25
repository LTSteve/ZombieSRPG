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

    private Transform target;

    private LerpedFloat aim = new LerpedFloat(1f, 0f);
    private LerpedVector targetPosition = new LerpedVector(1f, Vector3.zero);

    private Vector3 defaultLocalPosition;

    private void Start()
    {
        defaultLocalPosition = transform.localPosition;
    }

    public void LockTarget(Transform toTarget)
    {
        target = toTarget;

        //use local position
        targetPosition.SetValue(transform.worldToLocalMatrix * target.position);
        aim.SetValue(1f);
    }

    public void UnlockTarget()
    {
        targetPosition.SetValue(defaultLocalPosition);
        aim.SetValue(0f);
    }

    private void Update()
    {
        if(target != null)
            //keep track of target
            targetPosition.SetValue(transform.worldToLocalMatrix * target.position); //use local position

        if(headAimRig != null)
            headAimRig.weight = Mathf.Clamp01(aim.GetValue(Time.deltaTime));
        if (bodyAimRig != null)
            bodyAimRig.weight = Mathf.Clamp01(aim.GetValue(Time.deltaTime));
        transform.localPosition = targetPosition.GetValue(Time.deltaTime); //use local position
    }
}
