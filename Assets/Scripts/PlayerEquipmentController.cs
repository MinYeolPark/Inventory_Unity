using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerEquipmentController : MonoBehaviour
{
    [Header("Inventory Components")]
    [Space(2)]
    [SerializeField] private Inventory inventory;

    [Header("Animation Components")]    
    [SerializeField] private Animator animator;
    [SerializeField] private Animations animations;

    [Header("Anchors")]
    [SerializeField] private Transform helmetAnchor;
    [SerializeField] private Transform leftHandAnchor;
    [SerializeField] private Transform rightHandAnchor;
    [SerializeField] private Transform armorAnchor;

    [Space(5)]
    [Header("Equipments")]
    private GameObject currentHelmetObj;
    private GameObject currentLeftHandObj;
    private GameObject currentRighHandObj;
    private GameObject currentArmorObj;

    [Space(5)]
    [Header("Stats")]
    public int gold = 1000;
    public float hp, _hp;
    public float mp, _mp;
    public float ap;
    public float dp;

    private void Start()
    {
        _hp = 1000;
        hp = _hp;
        animator = GetComponent<Animator>();

        UIManager.instance.load();       

        inventory.init(this);        
        animations.init(this);
    }

    public void assignArmor(ArmorInventoryItem armor)
    {        
        switch (armor.armorType)
        {
            case ArmorType.HELMET:
                destroyIfNotNull(currentHelmetObj);
                currentHelmetObj = createNewItemInstance(armor, helmetAnchor);
                break;
            case ArmorType.TOP:
                destroyIfNotNull(currentArmorObj);
                currentArmorObj = createNewItemInstance(armor, armorAnchor);
                break;
        }

        transform.localPosition = Vector3.zero;
        transform.localRotation = new Quaternion(0, 180, 0, 0);
    }

    public void assignWeapon(WeaponInventoryItem weapon)
    {        
        switch (weapon.hand)
        {
            case Hand.LEFT:
                destroyIfNotNull(currentLeftHandObj);
                currentLeftHandObj = createNewItemInstance(weapon, leftHandAnchor);
                break;
            case Hand.RIGHT:
                destroyIfNotNull(currentRighHandObj);
                currentRighHandObj = createNewItemInstance(weapon, rightHandAnchor);
                break;
            case Hand.BOTH:
                destroyIfNotNull(currentLeftHandObj);
                destroyIfNotNull(currentRighHandObj);
                currentRighHandObj = createNewItemInstance(weapon, rightHandAnchor);
                currentLeftHandObj = currentRighHandObj;
                break;
            default:
                break;
        }

        animator.runtimeAnimatorController = weapon.overrideController;
        transform.localPosition = Vector3.zero;
        transform.localRotation = new Quaternion(0, 180, 0, 0);
    }

    public void assignConsumable(ConsumableInventoryItem consumable)
    {
        inventory.removeItem(consumable, 1);
        hp += consumable.getRecoveryPoints();
        Debug.Log($"Player hp = {hp}/{_hp}");
    }
    private void destroyIfNotNull(GameObject obj)
    {
        if (obj)
        {
            Destroy(obj);
        }
    }
    private GameObject createNewItemInstance(InventoryItem item, Transform anchor)
    {
        var itemInstance = Instantiate(item.getPrefab(), anchor);
        itemInstance.transform.localPosition = item.getLocalPosition();
        itemInstance.transform.localRotation = item.getLocalRotation();
        return itemInstance;
    }
}
