using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Inventory System/Items/Consumable")]
public class ConsumableInventoryItem : InventoryItem
{
    [SerializeField] private int recoveryPoint;
    public override void assignItemToPlayer(PlayerEquipmentController playerEquipment)
    {
        playerEquipment.assignConsumable(this);
    }
    public override void resignItemToPlayer(PlayerEquipmentController playerEquipment)
    {
        playerEquipment.resignConsumable(this);
    }
    public override string getType()
    {
        return "POTION";
    }
    public int getRecoveryPoints()
    {
        return recoveryPoint;
    }
}
