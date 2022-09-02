using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDetailUI : iPopupAnimation
{
    public void Start()
    {
        style = iPopupStyle.zoom;
        state = iPopupState.close;
        openPoint = new Vector2(MainCamera.devWidth / 2, -60);
        closePoint = new Vector2(400, -60);
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
    }
    public void init(Inventory inventory)
    {
        //popup
        transform.localScale = Vector3.zero;
        methodOpen = null;
        methodClose = null;
    }
}
