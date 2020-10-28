using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EntityRigStatemachine : MonoBehaviour
{
    [SerializeField]
    private Rig headAimRig = null;
    [SerializeField]
    private Rig bodyAimRig = null;
    [SerializeField]
    private Rig weaponAimRig = null;
    [SerializeField]
    private Rig weaponIdleRig = null;

    private LerpedFloat aimState = new LerpedFloat(0.5f, 0f);
    private bool aimingWeapon = false;

    public void Holding()
    {
        aimState.SetValue(0f);
        aimingWeapon = false;
    }

    public void Aiming(bool weapon = false)
    {
        aimState.SetValue(1f);
        aimingWeapon = weapon;
    }

    public bool IsFullyAimed()
    {
        return aimState.GetValue() == 1f;
    }

    private void Update()
    {
        if (headAimRig != null)
            headAimRig.weight = Mathf.Clamp01(aimState.GetValue(Time.deltaTime));
        if (bodyAimRig != null)
            bodyAimRig.weight = Mathf.Clamp01(aimState.GetValue(Time.deltaTime));
        if (weaponAimRig != null)
        {
            if (aimingWeapon)
                weaponAimRig.weight = Mathf.Clamp01(aimState.GetValue(Time.deltaTime));
            else
                weaponAimRig.weight = Mathf.Clamp01(weaponAimRig.weight - Time.deltaTime);
        }
        if (weaponIdleRig != null)
        {
            if (aimingWeapon)
                weaponIdleRig.weight = Mathf.Clamp01(1f - aimState.GetValue(Time.deltaTime));
            else
                weaponIdleRig.weight = Mathf.Clamp01(weaponIdleRig.weight + Time.deltaTime);
        }
    }
}