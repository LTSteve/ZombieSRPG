using UnityEngine;

//[CreateAssetMenu(fileName = "Weapon Data", menuName = "Game Data/Weapon")] -- this is gonna be a base class so i don't need to add a menu option for it
public class WeaponData : ScriptableObject
{
    public float Damage = 1f;
    public float Rate = 1f;
    public bool TwoHanded = false;
}