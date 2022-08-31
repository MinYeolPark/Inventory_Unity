using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Inventory System/Items/Equipment")]
public class ArmorInventoryItem : InventoryItem
{
    public ArmorType armorType;
    public override void assignItemToPlayer(PlayerEquipmentController playerEquipment)
    {
        playerEquipment.assignArmor(this);
    }
}

public enum ArmorType { HELMET, TOP };