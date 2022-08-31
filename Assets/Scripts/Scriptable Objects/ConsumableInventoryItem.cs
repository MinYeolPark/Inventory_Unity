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

    public int getRecoveryPoints()
    {
        return recoveryPoint;
    }
}
