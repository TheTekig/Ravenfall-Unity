using UnityEngine;

public enum ItemType
{
    Consumable,
    KeyItem,
    Ammo,
    Weapon
}


[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public string description;
    public Sprite examinePng;

    public ItemType itemType;

    public bool isStackable;
    public int maxStackSize = 1;

    public WeaponData weaponData;

    public int healAmount;

}
