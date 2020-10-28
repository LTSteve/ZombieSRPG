using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class BaseProjectile : MonoBehaviour, IProjectile
{
    [SerializeField]
    protected float lifespan = 3f;

    private Rigidbody body;

    private float damage;

    public void Launch(Vector3 direction, float speed, float damage)
    {
        body = GetComponent<Rigidbody>();

        body.velocity = direction * speed;

        this.damage = damage;
    }

    private void Update()
    {
        lifespan -= Time.deltaTime;

        if(lifespan <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var healthHaver = other.GetComponent<IHealthHaver>();
        if(healthHaver != null)
        {
            healthHaver.DealDamage(damage);
        }

        Destroy(gameObject);
    }
}