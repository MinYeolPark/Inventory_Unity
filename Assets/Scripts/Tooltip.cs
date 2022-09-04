using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : iPopupAnimation
{
    public float timer = 0f;
    public float sensitivity = 0.4f;
    [SerializeField] private GameObject tooltipGo;
    [SerializeField] private TMP_Text text;
    [SerializeField] private RectTransform rt;
    [SerializeField] private RectTransform parentRt;

    private static Tooltip _instance;
    public static Tooltip instance
    {
        get
        {
            if (_instance != null)
                return _instance;
            return _instance;
        }
    }
    private void Awake()
    {        
        _instance = this;

        text = GetComponentInChildren<TMP_Text>();
        rt = GetComponent<RectTransform>();
        parentRt = UIManager.instance.canvas.GetComponent<RectTransform>();
    }

    public void Start()
    {
        style = iPopupStyle.zoom;
        state = iPopupState.close;
        openPoint = new Vector2(0, 0);
        closePoint = new Vector2(0, 0);
        _aniDt = 0.25f;
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
    public void init()
    {
        show(false);

        //popup
        transform.localScale = Vector3.zero;
        methodOpen = cbOpen;
        methodClose = cbClose;
    }

    public void showTooltip(Vector2 position)
    {
        if (state != iPopupState.close)
            return;

        timer += Time.deltaTime;
        if (timer > sensitivity)
        {
            transform.SetAsLastSibling();
            openPoint = new Vector3(position.x + rt.sizeDelta.x / 2, position.y - rt.sizeDelta.y / 2);
            closePoint = new Vector3(position.x + rt.sizeDelta.x / 2, position.y + rt.sizeDelta.y / 2);
            timer = 0f;
            show(true);            
        }
    }

    public void hideTooltip()
    {        
        if (state != iPopupState.proc)
            return;
                
        show(false);
    }

    public void setContents(InventoryItem item)
    {
        text.text = item.getName();
    }
    public void cbOpen(iPopupAnimation pop)
    {
        //print("on Open" + pop.state);
    }
    public void cbClose(iPopupAnimation pop)
    {
        //print("on CLose " + pop.state);
    }
}
