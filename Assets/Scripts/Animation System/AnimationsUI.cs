using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AnimationsUI : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    private Animator animator;
    private void OnEnable()
    {
        //print("On enable");
        //load();
    }

    public void load()
    {
        buttons = new Button[3];
        GameObject go = Resources.Load<GameObject>("Prefabs/Button");
        for (int i = 0; i < 3; i++)
        {
            Instantiate(go, transform);
            buttons[i] = go.GetComponent<Button>();            
        }
    }

    public void init(Animations animations)
    {
        animator = animations.getAnimator();
        buttons = GetComponentsInChildren<Button>();
        
        for(int i=0;i<3;i++)
        {
            buttons[i].GetComponentInChildren<TMP_Text>().text = "Animation" + i;
        }
        //for(int i = 0; i < 3; i++)
        //{            
        //    buttons[i].onClick.AddListener(delegate { onButton(i); });
        //    print("add onbutton");
        //}
    }

    public void onButton(int i)
    {
        animator.SetTrigger("Animation" + i);
        animator.gameObject.transform.localPosition = Vector3.zero;
        animator.gameObject.transform.localRotation = new Quaternion(0, 180, 0, 0);
    }
    
}
