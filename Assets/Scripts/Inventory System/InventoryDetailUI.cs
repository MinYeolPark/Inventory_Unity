using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryDetailUI : iPopupAnimation
{
    private Inventory inventory;
    private InventoryItem curItem;
    [SerializeField] private Image sourceImg;
    [SerializeField] private Image targetImg;
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private Button btn;
    public void Start()
    {
        style = iPopupStyle.move;
        state = iPopupState.close;
        openPoint = new Vector2(MainCamera.devWidth, -60);
        closePoint = new Vector2(400, -60);
        _aniDt = 0.2f;
        aniDt = 0;
        selected = -1;
        bShow = false;
        methodOpen = null;
        methodClose = null;
    }
    private void Update()
    {
        paint(Time.deltaTime);
    }
    public void init(Inventory inventory)
    {
        show(false);

        //popup
        transform.localScale = Vector3.zero;
        methodOpen = null;
        methodClose = null;

        this.inventory = inventory;
        btn = inventory.getDetailUIObject().GetComponentInChildren<Button>();
        btn.onClick.AddListener(() => onAssignButton());
    }
    public void set(InventoryItem item)
    {      
        curItem = item;
        sourceImg.sprite = item.getSprite();
        headerText.text = item.getName();
        contentText.text = item.getDescription() + "\nType: " + item.getType();       
    }
    public void onAssignButton()
    {
        curItem.assignItemToPlayer(inventory.playerEquipment);
    }
}
