using UnityEngine;

public interface IProjectile
{
    public void Launch(Vector3 direction, float speed, float damage);
}