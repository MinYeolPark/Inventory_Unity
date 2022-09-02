using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : iPopupAnimation
{
    public float timer = 0f;
    public float sensitivity = 0.25f;
    [SerializeField] private GameObject tooltipGo;
    [SerializeField] private TMP_Text text;
    [SerializeField] private RectTransform rt;

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
    }

    public void Start()
    {
        style = iPopupStyle.zoom;
        state = iPopupState.close;
        openPoint = new Vector2(0, 0);
        closePoint = new Vector2(0, 0);
        _aniDt = 0.5f;
        aniDt = 0;
        selected = -1;
        bShow = false;
        methodOpen = null;
        methodClose = null;
    }
    private void Update()
    {
        paint(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.P))
            showTooltip(Input.mousePosition);
    }
    public void init()
    {
        show(false);

        //popup
        transform.localScale = Vector3.zero;
        methodOpen = null;
        methodClose = null;
    }

    public void showTooltip(Vector2 position)
    {
        show(true);
        print(position);
        openPoint = position;
        closePoint = position;
    }

    public void hideTooltip()
    {
        show(false);
    }
}
