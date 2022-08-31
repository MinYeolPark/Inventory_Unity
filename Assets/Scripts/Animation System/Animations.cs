using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{    
    [SerializeField] private AnimationsUI animationsUIPrefab;
    private AnimationsUI _animationsUI;
    private AnimationsUI animationsUI
    {
        get
        {
            if (!_animationsUI)
            {
                _animationsUI = Instantiate(animationsUIPrefab, UIManager.instance.canvas.transform);
            }
            return _animationsUI;
        }
    }
    private PlayerEquipmentController player;
    public void init(PlayerEquipmentController player)
    {
        this.player = player;
        openUI();
    }
    public void openUI()
    {
        animationsUI.gameObject.SetActive(!animationsUI.gameObject.activeSelf);
        animationsUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 260);
        animationsUI.init(this);
    }

    public Animator getAnimator()
    {
        return player.GetComponent<Animator>();
    }
}
