using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Gun : MonoBehaviour, IWeapon
{
    //a base speed for bullets, measured in units/sec
    //roughly equivalent to 700 m/s
    public static float BULLETSPEED = 700f;

    [SerializeField]
    public GunData GunData;
    [SerializeField]
    protected Transform bulletSpawnPoint;

    protected float timeSinceLastBullet;
    protected float timeSinceLastBurst;
    protected int burstState;

    private BoxCollider boxCollider;
    private SphereCollider sphereCollider;
    private Rigidbody body;

    public Transform GetTransform()
    {
        return transform;
    }

    protected void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        sphereCollider = GetComponent<SphereCollider>();
        body = GetComponent<Rigidbody>();
    }

    public void DisableInteractions()
    {
        boxCollider.enabled = false;
        sphereCollider.enabled = false;
        body.isKinematic = true;
        body.useGravity = false;
        body.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void EnableInteractions()
    {
        boxCollider.enabled = true;
        sphereCollider.enabled = true;
        body.isKinematic = false;
        body.useGravity = true;
        body.constraints = 0;
    }

    public virtual bool Shoot()
    {
        if (GunData == null || GunData.BulletPrefab == null) return false;

        if (!_updateBurstState()) return false;
        //burst is active

        return _updateShootState();
    }

    private bool _updateBurstState()
    {
        if (burstState >= GunData.BulletsPerBurst)//in between bursts
        {
            timeSinceLastBurst += Time.deltaTime;

            if (timeSinceLastBurst > GunData.BurstCooldown)
            {
                timeSinceLastBurst = 0f;
                burstState = 0;
            }

            return false;
        }

        return true;
    }

    private bool _updateShootState()
    {
        var shotTime = GunData.Rate == 0f ? Time.deltaTime : (1f / GunData.Rate);

        timeSinceLastBullet += Time.deltaTime;

        if (timeSinceLastBullet > shotTime)
        {
            timeSinceLastBullet = 0f;
            burstState++;
            _generateBullet();
            return true;
        }

        return false;
    }

    private void _generateBullet()
    {
        var newBullet = Instantiate(GunData.BulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).GetComponent<IProjectile>();

        newBullet.Launch(transform.forward, GetBulletTravelSpeed(), GunData.Damage);
    }

    public float GetBulletTravelSpeed()
    {
        return BULLETSPEED * GunData.BulletSpeed;
    }
}