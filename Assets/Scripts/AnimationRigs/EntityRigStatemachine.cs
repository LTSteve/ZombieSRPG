using System.Collections;
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

    //Gun Kicks
    [SerializeField]
    private MultiAimConstraint gunKickAimConstraint = null;
    [SerializeField]
    private MultiPositionConstraint gunKickPositionConstraint = null;

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

    private Coroutine pistonKickRoutine;
    private Coroutine verticalKickRoutine;

    public void Kick(GunData gunData)
    {
        if(gunData.KickType == GunData.GunKickType.None || gunData.Kick == 0)
        {
            return;
        }

        switch (gunData.KickType)
        {
            case GunData.GunKickType.Piston:
                if (pistonKickRoutine != null) StopCoroutine(pistonKickRoutine);
                pistonKickRoutine = StartCoroutine(_doPistonGunKick(gunData.Kick));
                break;
            case GunData.GunKickType.Vertical:
                if (verticalKickRoutine != null) StopCoroutine(verticalKickRoutine);
                verticalKickRoutine = StartCoroutine(_doVerticalGunKick(gunData.Kick));
                break;
        }
    }

    private IEnumerator _doPistonGunKick(float kick)
    {
        if (gunKickPositionConstraint != null)
        {
            gunKickPositionConstraint.weight = kick;

            for (var i = 0; i < 100 && gunKickPositionConstraint.weight != 0f; i++)
            {
                yield return null;

                gunKickPositionConstraint.weight = gunKickPositionConstraint.weight * 0.9f;
            }
        }

        gunKickPositionConstraint.weight = 0f;
        pistonKickRoutine = null;
    }
    private IEnumerator _doVerticalGunKick(float kick)
    {
        if(gunKickAimConstraint != null)
        {
            gunKickAimConstraint.weight = kick;

            for (var i = 0; i < 1000 && gunKickAimConstraint.weight != 0f; i++)
            {
                yield return null;

                gunKickAimConstraint.weight = gunKickAimConstraint.weight * 0.9f;
            }
        }

        gunKickAimConstraint.weight = 0f;
        verticalKickRoutine = null;
    }
}