using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public WeaponData meleeWeapon;
    public WeaponData rangedWeapon;

    private PlayerAttack playerAttack;
    private PlayerShooting playerShooting;

    [SerializeField] private WeaponData testWeapon; // Variável para teste temporário

    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerShooting = GetComponent<PlayerShooting>();

        //TESTE TEMPORARIO PARA VER SE O SISTEMA DE EQUIPAMENTO FUNCIONA
        if (testWeapon != null)
        {
            EquipWeapon(testWeapon);
        }
    }

    public void EquipWeapon(WeaponData weapon)
    {
        if (weapon.weaponType == WeaponType.Melee)
        {
            meleeWeapon = weapon;
            playerAttack.SetWeapon(weapon);
        }
        else if (weapon.weaponType == WeaponType.Ranged)
        {
            rangedWeapon = weapon;
            playerShooting.SetWeapon(weapon);
        }
    }
}
