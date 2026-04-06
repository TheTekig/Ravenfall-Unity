using UnityEngine;

public enum  WeaponType
{
    Melee,
    Ranged
}

[CreateAssetMenu(menuName = "Weapons/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;

    public WeaponType weaponType;

    public int damage;

    //Ranged
    public float fireRate;
    public GameObject projectile;

    public Item ammoType;
    public int ammoPerShot = 1;

    //Animation
    public RuntimeAnimatorController animatorOverride;
}
