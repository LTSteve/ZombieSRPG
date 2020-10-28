using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(LifespanEffect))]
public class BaseProjectile : MonoBehaviour, IProjectile
{
    private Rigidbody body;

    private float damage;

    public void Launch(Vector3 direction, float speed, float damage)
    {
        body = GetComponent<Rigidbody>();

        body.velocity = direction * speed;

        this.damage = damage;
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