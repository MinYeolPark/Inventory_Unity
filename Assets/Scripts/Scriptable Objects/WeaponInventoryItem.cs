using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Inventory System/Items/Weapon")]
public class WeaponInventoryItem : InventoryItem
{
    public AnimatorOverrideController overrideController;
    public Hand hand;
    public WeaponType weaponType;
    public override void assignItemToPlayer(PlayerEquipmentController playerEquipment)
    {
        playerEquipment.assignWeapon(this);
    }
}
public enum Hand { LEFT, RIGHT, BOTH };
public enum WeaponType { NONE, AXE, ONEHANDSWORD, GREATSWORD, SHIELD };