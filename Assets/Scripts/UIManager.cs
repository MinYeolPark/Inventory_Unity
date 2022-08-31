using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager instance
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
    }

    public Canvas canvas;
    public void load()
    {
        GameObject go;
        if (!canvas)
        {
            go = new GameObject("Canvas");
            //0
            Canvas c = go.AddComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            //1
            CanvasScaler cs;
            cs = go.AddComponent<CanvasScaler>();
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(MainCamera.devWidth, MainCamera.devHeight);

            go.AddComponent<GraphicRaycaster>();
            canvas = c;
        }

        //Back ground
        go = new GameObject("Back Ground");
        go.transform.SetParent(canvas.transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.AddComponent<RectTransform>().sizeDelta = new Vector2(1280, 720);
        go.AddComponent<CanvasRenderer>();
        
        Sprite sprite = Resources.Load<Sprite>("Sprites/Background");
        go.AddComponent<Image>().sprite = sprite;


        //Render Texture
        go = new GameObject("UI Character");
        go.transform.SetParent(canvas.transform);
        go.transform.localPosition = Vector3.zero;

        RawImage img = go.AddComponent<RawImage>();
        img.texture = Resources.Load<RenderTexture>("Textures/UICharacter");

        RectTransform rt;
        rt = go.transform.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(512, 512);
        rt.anchoredPosition = new Vector2(0, 0);        
    }
}
