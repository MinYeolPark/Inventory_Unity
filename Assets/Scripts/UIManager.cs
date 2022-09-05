using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum SND
{
    bgm_menu = 0,

    bgm_end,

    snd_click = bgm_end,
    snd_open,
    snd_close,

}

public enum PTC
{
    ptc_click,
}
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
    public float timer = 0f;
    public float sensitivity = 0.25f;
    [SerializeField] private Tooltip tooltipPrefab;
    private Tooltip _tooltip;
    private Tooltip tooltip
    {
        get
        {
            if (!_tooltip)
            {
                if (tooltipPrefab == null)
                {
                    tooltipPrefab = Resources.Load<Tooltip>("Prefabs/Tooltip");
                }
                _tooltip = Instantiate(tooltipPrefab, UIManager.instance.canvas.transform);
            }
            return _tooltip;
        }
    }
    [SerializeField] private GameObject loading;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private ParticleSystem[] particles;
    private void Start()
    {
        _instance = this;

        audioSource = GetComponent<AudioSource>();
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
            //c.renderMode = RenderMode.ScreenSpaceCamera;
            //c.worldCamera = Camera.main;
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
               
        AspectRatioFitter arf = go.AddComponent<AspectRatioFitter>(); ;        
        arf.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
        arf.aspectRatio = 1;

        RectTransform rt = go.transform.GetComponent<RectTransform>();
        //rt.sizeDelta = new Vector2(512, 512);
        rt.anchorMin = new Vector2(0, 0.5f);
        rt.anchorMax = new Vector2(1, 0.5f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localPosition = new Vector2(0f, 0f);

#if false
        //Loading Screen
        go = Resources.Load<GameObject>("Prefabs/Loading");
        Instantiate(go, canvas.transform);
        go.transform.localPosition = Vector3.zero;

        rt = go.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector3.zero;
        rt.anchorMin = Vector3.zero;
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 0.5f);

        loading = go;
#endif
        //Sounds
        sounds = Resources.LoadAll<AudioClip>("Sound");

        //Particle
        particles = Resources.LoadAll<ParticleSystem>("Prefabs/Effects/Click");

        //Tooltip
        tooltip.init();
    }

    public void soundPlay(SND index)
    {
        if(index < SND.bgm_end)
        {
            audioSource.clip = sounds[(int)index];
            audioSource.loop = true;
            audioSource.Play();
        }
        audioSource.PlayOneShot(sounds[(int)index]);
    }

    public void setVolume(float volume)
    {
        audioSource.volume = volume;
    }
    public void particlePlay(PTC index, Vector2 point)
    {        

    }
}
 