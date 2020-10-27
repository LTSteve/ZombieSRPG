using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class AbstractWeapon : MonoBehaviour, IWeapon
{
    //a base speed for bullets, measured in units/sec
    //roughly equivalent to 700 m/s
    public static float BULLETSPEED = 700f;

    [SerializeField]
    protected Transform bullet;
    [SerializeField]
    protected Transform bulletSpawnPoint;
    [SerializeField]
    protected float bulletSpeed;
    [SerializeField]
    protected float bulletBurst;
    [SerializeField]
    protected float burstBulletsPerSecond;
    [SerializeField]
    protected float burstCooldown;
    [SerializeField]
    protected float damage;

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

    public virtual void Shoot()
    {
        if (bullet == null) return;

        if (!_updateBurstState()) return;
        //burst is active

        _updateShootState();
    }

    private bool _updateBurstState()
    {
        if (burstState >= bulletBurst)//in between bursts
        {
            timeSinceLastBurst += Time.deltaTime;

            if (timeSinceLastBurst > burstCooldown)
            {
                timeSinceLastBurst = 0f;
                burstState = 0;
            }

            return false;
        }

        return true;
    }

    private void _updateShootState()
    {
        var shotTime = burstBulletsPerSecond == 0f ? Time.deltaTime : (1f / burstBulletsPerSecond);

        timeSinceLastBullet += Time.deltaTime;

        if (timeSinceLastBullet > shotTime)
        {
            timeSinceLastBullet = 0f;
            burstState++;
            _generateBullet();
        }
    }

    private void _generateBullet()
    {
        var newBullet = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation).GetComponent<IProjectile>();

        newBullet.Launch(transform.forward, GetBulletTravelSpeed(), damage);
    }

    public float GetBulletTravelSpeed()
    {
        return BULLETSPEED * bulletSpeed;
    }
}