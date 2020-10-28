using Unity.Collections;
using UnityEngine;

public class BaseHealthHaver : MonoBehaviour, IHealthHaver
{
    [SerializeField]
    private float maxHealth = 100f;
    [SerializeField]
    private float spawnHealth = 100f;
    [SerializeField]
    private float health;

    private void Start()
    {
        health = spawnHealth;
    }

    public void DealDamage(float damage)
    {
        health -= damage;
        if(health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}