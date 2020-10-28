using UnityEngine;
using UnityEngine.AI;

public class AimAndShootAction : IEntityAction
{
    //manage target redicle
    private TargetingEffect targetingRedicle;

    //read rig state
    private EntityRigStatemachine entityRigStatemachine;

    //manage rotation
    private KeepAlignedAction keepAligned;

    //shoot weapon
    private IWeapon weaponToShoot;

    private bool validSetup = false;

    public AimAndShootAction(Transform targetToShootAt, IEntity entityHoldingWeapon, IWeapon weaponToShoot)
    {
        targetingRedicle = entityHoldingWeapon?.GetTargeting();
        entityRigStatemachine = entityHoldingWeapon?.GetRigStatemachine();

        if (targetingRedicle != null)
            keepAligned = new KeepAlignedAction(entityHoldingWeapon, targetingRedicle.transform);

        this.weaponToShoot = weaponToShoot;

        targetingRedicle?.LockTarget(targetToShootAt, true, weaponToShoot == null ? 1f : weaponToShoot.GetBulletTravelSpeed());

        validSetup = weaponToShoot != null && targetToShootAt != null;
    }

    public void Abort()
    {
        targetingRedicle.UnlockTarget();
    }

    public bool IsDone()
    {
        return !validSetup || targetingRedicle.TargetIsDead();
    }

    public void Update()
    {
        if (IsDone()) return;

        keepAligned.Update();

        //aiming complete & not adjusting rotation
        if (entityRigStatemachine.IsFullyAimed() && keepAligned.IsWithinOuterLimit())
        {
            //call weapon shoot
            var shot = weaponToShoot.Shoot();

            if (shot && weaponToShoot is Gun)
                entityRigStatemachine.Kick(((Gun)weaponToShoot).GunData);
        }
    }
}