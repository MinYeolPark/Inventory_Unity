using UnityEngine;

public abstract class InventoryItem : ScriptableObject
{
    [Header("itemDetail")]
    [Multiline]
    [SerializeField] private string description;

    [Header("itemData")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private string itemName;
    [SerializeField] private Vector3 itemLocalPosition;
    [SerializeField] private Vector3 itemLocalRotation;

    public string getDescription()
    {
        return description;
    }
    public Sprite getSprite()
    {
        if (itemSprite != null)
            return itemSprite;
        else
        {
            Debug.Log("item sprite empty");
            return null;
        }
    }

    public string getName()
    {
        if (itemSprite != null)
            return itemName;
        else
        {
            Debug.Log("item name empty");
            return null;
        }
    }

    public GameObject getPrefab()
    {
        if (itemPrefab != null)
            return itemPrefab;
        else
        {
            Debug.Log("item prefab empty");
            return null;
        }
    }

    public Vector3 getLocalPosition()
    {
        return itemLocalPosition;
    }

    public Quaternion getLocalRotation()
    {
        return Quaternion.Euler(itemLocalRotation);
    }

    public abstract string getType();
   
    public abstract void assignItemToPlayer(PlayerEquipmentController playerEquipment);
    public abstract void resignItemToPlayer(PlayerEquipmentController playerEquipment);
}
