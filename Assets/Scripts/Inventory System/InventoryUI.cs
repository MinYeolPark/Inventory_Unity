using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class InventoryUI : iPopupAnimation
{
    [SerializeField] private int maxX = 4;
    [SerializeField] private int maxY = 5;
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryDetailUI detail;
    [SerializeField] private Transform slotsParent;
    [SerializeField] private InventorySlot slotPrefab;
    [SerializeField] private InventorySlot selectedItem;
    [SerializeField] private Dictionary<InventoryItem, InventorySlot> itemToSlotMap = new Dictionary<InventoryItem, InventorySlot>();

    public void Start()
    {
        style = iPopupStyle.zoomRotate;
        state = iPopupState.close;
        openPoint = new Vector2(-MainCamera.devWidth / 2, -60);
        closePoint = new Vector2(-350, -60);
        _aniDt = 0.5f;
        aniDt = 0;
        selected = -1;
        bShow = false;
        methodOpen = null;
        methodClose = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (bShow == false)
            {
                show(true);

                UIManager.instance.soundPlay(SND.snd_open);
            }
            else if (state == iPopupState.proc)
            {                
                detail.show(false);
                show(false);

                Tooltip.instance.hideTooltip();
                UIManager.instance.soundPlay(SND.snd_close);
            }

        }
        paint(Time.deltaTime);        
    }
    public void init(Inventory inventory)
    {        
        this.inventory = inventory;
        GridLayoutGroup glg = UIManager.instance.canvas.GetComponentInChildren<GridLayoutGroup>();
        slotsParent = glg.transform;
        glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        glg.constraintCount = maxX;

        detail = inventory.getDetailUIObject().GetComponent<InventoryDetailUI>();        

        Button[] btns = GetComponentsInChildren<Button>();        
        btns[0].onClick.AddListener(() => show(false));
        btns[1].onClick.AddListener(arrangement);
        btns[1].GetComponentInChildren<TMP_Text>().text = "Arrangement";
        btns[2].onClick.AddListener(sortByName);
        btns[2].GetComponentInChildren<TMP_Text>().text = "Sort By Name";
        btns[3].onClick.AddListener(removeAll);
        btns[3].GetComponentInChildren<TMP_Text>().text = "Remove All";

        for(int i=0;i<btns.Length;i++)
        {
            if(i==0)
            {
                btns[i].onClick.AddListener(() => UIManager.instance.soundPlay(SND.snd_close));                
                btns[i].onClick.AddListener(() => detail.show(false));                
                continue;
            }
            btns[i].onClick.AddListener(() => UIManager.instance.soundPlay(SND.snd_open));
        }
        ///////////////////////////////////////////////////

        MainCamera.methodMouse = cbMouse;
        var itemsMap = inventory.getAllItemsMap();
        foreach (var kvp in itemsMap)
        {
            createOrUpdateSlot(inventory, kvp.Key, kvp.Value);
        }        

        //create empty Slots
        for (int i = 0; i < maxX * maxY - itemToSlotMap.Count; i++)
        {
            var empty = createSlot(inventory, null, 0);            
        }
        //popup
        transform.localScale = Vector3.zero;
        methodOpen = cbInventoryOpen;
        methodClose = cbInventoryClose;
    }
    public void createOrUpdateSlot(Inventory inventory, InventoryItem item, int itemCount)
    {
        if (!itemToSlotMap.ContainsKey(item))
        {
            var slot = createSlot(inventory, item, itemCount);
            itemToSlotMap.Add(item, slot);
        }
        else
        {
            updateSlot(item, itemCount);
        }
    }
    public void updateSlot(InventoryItem item, int itemCount)
    {
        itemToSlotMap[item].update(itemCount);
    }
    private InventorySlot createSlot(Inventory inventory, InventoryItem item, int itemCount)
    {
        var slot = Instantiate(slotPrefab, slotsParent);
        if (item != null)
        {
            slot.init(item.getSprite(), item.getName(), itemCount);
        }
        else
        {
            slot.init(null, null, 0);
        }
        return slot;
    }
    public void moveSlot(InventorySlot resource, InventorySlot target)
    {
        if (resource == null || target == null) return;

        InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();
        int x = -1, y = -1;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Equals(resource))
            {
                x = slots[i].transform.GetSiblingIndex();                
            }
            if (slots[i].Equals(target))
            {
                y = slots[i].transform.GetSiblingIndex();
            }
            if (x != -1 && y != -1) break;
        }        
        slots[x].transform.SetSiblingIndex(y);
        slots[y].transform.SetSiblingIndex(x);        
    }
    public void destroySlot(InventoryItem item)
    {
        Sprite sprite = Resources.Load<Sprite>("Sprites/Default Slot");        
        itemToSlotMap[item].init(sprite, null, 0);
        itemToSlotMap.Remove(item);        
        arrangement();        
    }
    //////////////////////////////////////////////    
    //Sorting
    //////////////////////////////////////////////
    public void arrangement()
    {
        InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();
        int n = 0;
        for(int i=0; i<slots.Length; i++)
        {
            if (slots[i].getItemCount() != 0)
            {
                slots[i].transform.SetSiblingIndex(n);
                n++;
            }
        }
    }
    public void sortByName()
    {
        var orderedItems = itemToSlotMap.OrderBy(i => i.Key.getName()).ToList();
        InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();

        int n = 0;
        for (int i = 0; i < orderedItems.Count; i++)
        {
            orderedItems[i].Value.transform.SetSiblingIndex(n);
            n++;            
        }        
    }
    
    public void removeAll()
    {
        foreach (var item in inventory.getAllItemsMap().Keys)
        {
            inventory.resignItem(item);
        }        
        UIManager.instance.soundPlay(SND.snd_open);
    }

    //////////////////////////////////////////////
    //Inputs
    //////////////////////////////////////////////
    public void cbMouse(iKeystate stat, Vector3 point)
    {
        if (state != iPopupState.proc) return;          //Prevent inputs except state.proc
        if (stat == iKeystate.Enter)
        {
            onEnter(stat, point);
        }
        else if (stat == iKeystate.Exit)
        {
            onExit(stat, point);
        }
        else if (stat == iKeystate.Began)
        {
            onClick(stat, point);
        }
        else if (stat == iKeystate.Ended)
        {
            onEnd(stat, point);
        }
        else if (stat == iKeystate.Moved)
        {
            onMove(stat, point);
        }
        else if (stat == iKeystate.DoublieClick)
        {
            onDoubleClick(stat, point);
        }
    }
    public void onEnter(iKeystate stat, Vector2 point)
    {        
        var itemsMap = inventory.getAllItemsMap();
        foreach (var item in itemsMap)
        {
            if (item.Value == 0)
                continue;
            if (itemToSlotMap[item.Key].containPointSlot(point))
            {
                Tooltip.instance.setContents(item.Key);
                break;
            }
        }

        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (UIManager.instance.canvas.GetComponent<RectTransform>()
            , point, null, out anchoredPos); // null = UIManager.instance.canvas.renderMode == RenderMode.ScreenSpaceOverlay?

        Tooltip.instance.showTooltip(anchoredPos);
    }

    public void onExit(iKeystate stat, Vector2 point)
    {        
        Tooltip.instance.hideTooltip();
    }

    public void onClick(iKeystate stat, Vector2 point)
    {
        if (selectedItem != null) return;

        var itemsMap = inventory.getAllItemsMap();        
        foreach (var item in itemsMap)
        {            
            if (item.Value==0)
            {
                itemsMap.Remove(item.Key);
                selectedItem = null;
                                
                //detail.set(null);
                detail.show(false);
                continue;
            }

            if (itemToSlotMap[item.Key].containPointSlot(point))
            {
                selectedItem = itemToSlotMap[item.Key];

                detail.set(item.Key);
                detail.show(true);
                break;
            }
        }
    }
    public void onMove(iKeystate stat, Vector2 point)
    {
        if (selectedItem != null)
        {
            GameObject hg = selectedItem.createHoverSlot(selectedItem);
            hg.transform.position = point;
        }
    }
    public void onDoubleClick(iKeystate stat, Vector2 point)
    {
        var itemsMap = inventory.getAllItemsMap();
        foreach (var item in itemsMap)
        {
            if (item.Value == 0)
                continue;

            if (itemToSlotMap[item.Key].containPointSlot(point))
            {
                inventory.assignItem(item.Key);
                break;
            }
        }
    }
    public void onEnd(iKeystate stat, Vector2 point)
    {
        var itemsMap = inventory.getAllItemsMap();
        InventorySlot[] slots = GetComponentsInChildren<InventorySlot>();


#if false //only swap item each                
        foreach (var item in itemsMap)
        {
            if (itemToSlotMap[item.Key].containPointSlot(point))
            {
                moveSlot(selectedItem, itemToSlotMap[item.Key]);        
                break;
            }
        }
#else
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].containPointSlot(point))
            {
                moveSlot(selectedItem, slots[i]);
                break;
            }
        }
#endif

        if (selectedItem != null)
        {
            selectedItem.destroyHoverSlot();
            selectedItem = null;
        }
    }

    void cbInventoryOpen(iPopupAnimation pop)
    {
        print("inventory opened");
    }
    void cbInventoryClose(iPopupAnimation pop)
    {
        print("inventory closed");
    }    
}