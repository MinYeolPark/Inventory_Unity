using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemCountText;
    [SerializeField] private string itemName;
    [SerializeField] private Button slotButton;
        
    public void init(Sprite itemSprite, string itemName, int itemCount)
    {
        rectTransform = GetComponent<RectTransform>();
        if (itemSprite != null)
            itemImage.sprite = itemSprite;
        this.itemName = itemName;
        update(itemCount);
    }

    public void update(int itemCount)
    {
        itemCountText.text = itemCount.ToString();
    }

    public void assingSlotButtonCallback(System.Action onClickCallback)
    {
        slotButton.onClick.AddListener(() => onClickCallback());
    }

    public bool containPointSlot(Vector2 point)
    {
        if (point.x > transform.position.x - rectTransform.sizeDelta.x / 2 &&
           point.x < transform.position.x + rectTransform.sizeDelta.x / 2 &&
           point.y > transform.position.y - rectTransform.sizeDelta.y / 2 &&
           point.y < transform.position.y + rectTransform.sizeDelta.y / 2)
        {
            return true;
        }
        return false;
    }
    public int getItemCount()
    {        
        return int.Parse(itemCountText.text);
    }
    public string getItemName()
    {
        return itemName;
    }

    [SerializeField] private HoverSlot hoverSlot;
    public GameObject createHoverSlot(InventorySlot selectedSlot)
    {
        if (hoverSlot.hoverGo != null)
            return hoverSlot.hoverGo;

        GameObject go = new GameObject("Hover slot");

        Image img = go.AddComponent<Image>();
        img.sprite = selectedSlot.itemImage.sprite;
        img.rectTransform.sizeDelta = new Vector2(40, 40);
        img.color = new Color(1f, 1f, 1f, 0.8f);

        go.transform.SetParent(UIManager.instance.canvas.transform);
        go.transform.position = Input.mousePosition;

        hoverSlot.hoverGo = go;
        return hoverSlot.hoverGo;
    }
    public void destroyHoverSlot()
    {
        Destroy(hoverSlot.hoverGo);
    }
}

[System.Serializable]
public class HoverSlot
{
    public GameObject hoverGo;
    public Image hoverImg;
}