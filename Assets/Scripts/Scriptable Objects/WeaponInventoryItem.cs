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

    public override void resignItemToPlayer(PlayerEquipmentController playerEquipment)
    {
        playerEquipment.resignWeapon(this);
    }
    public override string getType()
    {
        return weaponType.ToString();
    }
}
public enum Hand { LEFT, RIGHT, BOTH };
public enum WeaponType { NONE, AXE, SWORD, GREATSWORD, SHIELD };