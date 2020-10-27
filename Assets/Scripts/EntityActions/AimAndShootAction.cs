using UnityEngine;
using UnityEngine.AI;

public class AimAndShootAction : IEntityAction
{
    private AimAtAction aimAction;
    private IWeapon toShoot;
    private Transform toShootAt;

    private float bulletSpeed;

    private bool validSetup;

    public AimAndShootAction(Transform toLookAt, IEntity targeter, IWeapon toShoot)
    {
        aimAction = new AimAtAction(toLookAt, targeter, true, 1f);

        this.toShoot = toShoot;
        toShootAt = toLookAt;
        bulletSpeed = toShoot == null ? 1f : toShoot.GetBulletTravelSpeed();

        validSetup = toShoot != null;
    }

    public void Abort()
    {
        aimAction.Abort();
    }

    public bool IsDone()
    {
        return false;
    }

    public void Update()
    {
        aimAction.UpdateOvershootSeconds(Vector3.Distance(toShootAt.position,toShoot.GetTransform().position) / bulletSpeed);

        aimAction.Update();

        if (!validSetup) return;

        if (aimAction.IsFullyAimed())
        {
            //call weapon shoot
            toShoot.Shoot();
        }
    }
}