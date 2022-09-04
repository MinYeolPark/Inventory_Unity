using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MainCamera : MonoBehaviour
{
    public static MainCamera mainCamera;

    public static int devWidth = 1280, devHeight = 720;     //HD

    //Inputs
    public static MethodMouse methodMouse = null;
    public static MethodWheel methodWheel = null;
    public static MethodKeyboard methodKeyboard = null;


    public bool isClicked = false;
    public float tapTimer = 0f;
    public float tapSensitivity = 0.2f;

    Vector3 prevV;
    bool drag = false;

    PlayerEquipmentController player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    void Start()
    {
        mainCamera = this;
        player = FindObjectOfType<PlayerEquipmentController>();
        offset = transform.position - player.transform.position;
    }

    void Update()
    {
        setResolutionClip(devWidth, devHeight);

        updateMouse();
        //updateKeyboard();
        //updateWheel();
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = player.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    void updateMouse()
    {
        if (isClicked && Time.time - tapTimer > tapSensitivity)
            isClicked = false;

        int btn = 0;// 0:left, 1:right, 2:wheel, 3foward, 4back
        bool doubeClk = false;
        if (Input.GetMouseButtonDown(btn))
        {
            if (isClicked && (Time.time - tapTimer) < tapSensitivity)
            {
                doubeClk = true;
                isClicked = false;
            }
            else// if (!isClicked)
            {
                tapTimer = Time.time;
                isClicked = true;
            }
            Vector3 v = Input.mousePosition;
            drag = true;
            prevV = Input.mousePosition;// 누르자 말자 Moved 안들어오게 방지
            if (methodMouse != null)
                methodMouse(doubeClk ? iKeystate.DoublieClick : iKeystate.Began, v);
        }
        else if (Input.GetMouseButtonUp(btn))
        {
            Vector3 v = Input.mousePosition;
            drag = false;

            if (methodMouse != null)
                methodMouse(iKeystate.Ended, v);
        }

        if (drag)
        {
            Vector3 v = Input.mousePosition;
            if (prevV == v)
                return;
            prevV = v;

            if (methodMouse != null)
                methodMouse(iKeystate.Moved, v);
        }
#if true

        if(isMouseOverUI())
        {
            Vector3 v = Input.mousePosition;
            if (methodMouse != null)
                methodMouse(iKeystate.Enter, v);
        }
        else
        {
            Vector3 v = Input.mousePosition;
            if (methodMouse != null)
                methodMouse(iKeystate.Exit, v);
        }
#endif
    }
    void updateWheel()
    {
        if (methodWheel == null)
            return;

        if (Input.mouseScrollDelta != Vector2.zero)
        {
            methodWheel(new Vector2 (Input.mouseScrollDelta.x,
                                    Input.mouseScrollDelta.y));
        }
    }
    void updateKeyboard()
    {
        if (methodKeyboard == null)
            return;

        for (int i = 0; i < kc.Length; i++)
        {
            KeyCode c = kc[i];

            if (Input.GetKeyDown(c))
                methodKeyboard(iKeystate.Began, (iKeyboard)i);
            else if (Input.GetKeyUp(c))
                methodKeyboard(iKeystate.Ended, (iKeyboard)i);
            else if (Input.GetKey(c))
                methodKeyboard(iKeystate.Moved, (iKeyboard)i);
        }
    }
    private KeyCode[] kc = new KeyCode[]
    {
        KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.UpArrow, KeyCode.DownArrow,
        KeyCode.I,
        KeyCode.Space
    };


    public void setResolutionClip(int devWidth, int devHeight)
    {
        Screen.SetResolution(devWidth, devHeight, false);
        float r0 = (float)devWidth / devHeight;

        int width = Screen.width, height = Screen.height;
        float r1 = (float)width / height;

        if (r0 < r1)// 세로가 길때
        {
            float w = r0 / r1;
            float x = (1 - w) / 2;
            Camera.main.rect = new Rect(x, 0, w, 1);
        }
        else// 가로가 길때
        {
            float h = r1 / r0;
            float y = (1 - h) / 2;
            Camera.main.rect = new Rect(0, y, 1, h);
        }
    }

    bool isMouseOverUI()
    {        
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);
        for (int i = 0; i < result.Count; i++)
        {
            if (result[i].gameObject.GetComponent<InventorySlot>() == null
                || result[i].gameObject.GetComponent<InventorySlot>().getItemCount() == 0)
            {
                result.RemoveAt(i);
                i--;
            }
        }
        return result.Count > 0;
    }    
}
public enum iKeystate
{
    Enter = 0,
    Exit,
    Began,      // pressed
    Moved,      // moved
    Ended,      // released
    DoublieClick,
};

public delegate void MethodMouse(iKeystate stat, Vector3 point);
public delegate void MethodWheel(Vector2 wheel);

public enum iKeyboard
{
    Left = 0,// a, A, 4, <-
    Right,
    Up,
    Down,
    Space
};

public delegate void MethodKeyboard(iKeystate stat, iKeyboard key);
