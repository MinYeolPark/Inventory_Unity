using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField] private List<InventoryItemContainer> items = new List<InventoryItemContainer>();
    [SerializeField] private InventoryUI inventoryUIPrefab;
    private InventoryUI _inventoryUI;
    private InventoryUI inventoryUI
    {
        get
        {
            if (!_inventoryUI)
            {
                _inventoryUI = Instantiate(inventoryUIPrefab, UIManager.instance.canvas.transform);
            }
            return _inventoryUI;
        }
    }
    private Dictionary<InventoryItem, int> itemToCountMap = new Dictionary<InventoryItem, int>();
    private PlayerEquipmentController playerEquipment;

    public void init(PlayerEquipmentController playerEquipment)
    {
        this.playerEquipment = playerEquipment;

        RectTransform rt;
        rt = inventoryUI.GetComponent<RectTransform>();
        rt.localPosition = new Vector2(-350, -55);
        for (int i = 0; i < items.Count; i++)
        {
            itemToCountMap.Add(items[i].getItem(), items[i].getItemCount());
        }
        inventoryUI.init(this);
    }
    public void openUI()
    {        
        inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);

        //animation
    }
    public void assignItem(InventoryItem item)
    {
        Debug.Log(item + "assigned");
        item.assignItemToPlayer(playerEquipment);        
    }
    public Dictionary<InventoryItem, int> getAllItemsMap()
    {
        return itemToCountMap;
    }
    public void addItem(InventoryItem item, int count)
    {
        int currentItemCount;
        if (itemToCountMap.TryGetValue(item, out currentItemCount))
        {
            itemToCountMap[item] = currentItemCount + count;
        }
        else
        {
            itemToCountMap.Add(item, count);
        }
        inventoryUI.createOrUpdateSlot(this, item, count);
    }
    public void removeItem(InventoryItem item, int count)
    {
        int currentItemCount;
        if (itemToCountMap.TryGetValue(item, out currentItemCount))
        {
            itemToCountMap[item] = currentItemCount - count;
            Debug.Log(itemToCountMap[item]);
            if (currentItemCount - count <= 0)
            {
                inventoryUI.destroySlot(item);
            }
            else
            {
                inventoryUI.updateSlot(item, currentItemCount - count);
            }
        }
        else
        {
            Debug.Log(string.Format("Cant remove {0}. This item is not in the inventory"));
        }
    }
    public InventoryUI getUIObject()
    {
        return inventoryUI;
    }
}
