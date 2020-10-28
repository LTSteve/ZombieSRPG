using UnityEngine;

[CreateAssetMenu(fileName = "Gun Data", menuName = "Game Data/Weapons/Gun")]
public class GunData : WeaponData
{
    public float BulletSpeed;
    public int BulletsPerBurst;
    public float BurstCooldown;
    public Transform BulletPrefab;
}