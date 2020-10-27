using UnityEngine;

public interface IWeapon
{
    Transform GetTransform();
    void EnableInteractions();
    void DisableInteractions();
    void Shoot();
    float GetBulletTravelSpeed();
}