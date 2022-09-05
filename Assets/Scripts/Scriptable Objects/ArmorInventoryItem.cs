using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Inventory System/Items/Equipment")]
public class ArmorInventoryItem : InventoryItem
{
    public ArmorType armorType;
    public override void assignItemToPlayer(PlayerEquipmentController playerEquipment)
    {
        playerEquipment.assignArmor(this);
    }
    public override void resignItemToPlayer(PlayerEquipmentController playerEquipment)
    {
        playerEquipment.resignArmor(this);
    }
    public override string getType()
    {
        return armorType.ToString();
    }
}

public enum ArmorType { HELMET, TOP };