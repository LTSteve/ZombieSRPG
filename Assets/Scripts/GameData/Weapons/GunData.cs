using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun Data", menuName = "Game Data/Weapons/Gun")]
public class GunData : WeaponData
{
    [Range(0f, 1f)]
    public float BulletSpeed;
    public int BulletsPerBurst;
    public float BurstCooldown;
    public Transform BulletPrefab;
    [Range(0f, 1f)]
    public float Kick;
    public GunKickType KickType;

    public enum GunKickType
    {
        None,
        Vertical,
        Piston
    }
}