using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance
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
    [SerializeField] private Inventory inventory;
    [SerializeField] private Animations animations;


    [SerializeField] private GameState gameState;
    [SerializeField] private MethodLoading methodStart, methodLoaded;
    [SerializeField] private float loadingDt = 0f;
    [SerializeField] private float loadingSpeed = 200f;

    private void Start()
    {
        UIManager.instance.load();      //asset bundle?
        initGame();

        setLoading(GameState.GameStateMenu, null, null);
    }
    private void Update()
    {
        if(loadingDt !=0f)
        {
            loadingDt += loadingSpeed * Time.deltaTime;
        }
    }
    public void setLoading(GameState state, MethodLoading start, MethodLoading end)
    {
        switch(state)
        {
            case GameState.GameStateNone:
                break;
            case GameState.GameStateMenu:                
                UIManager.instance.soundPlay(SND.bgm_menu);
                UIManager.instance.setVolume(0.5f);
                break;
            case GameState.GameStateMap:
                break;
            case GameState.GameStateEnd:
                break;
        }
    }

    public void initGame()
    {
        PlayerEquipmentController player = Resources.Load<PlayerEquipmentController>("Prefabs/Player");
        PlayerEquipmentController playerGo = Instantiate(player);
        playerGo.transform.position = Vector3.zero;
        FindObjectOfType<MainCamera>().init(playerGo);
    }
}

public enum GameState
{
    GameStateMenu = 0,
    GameStateMap,
    GameStateEnd,

    GameStateNone,
};

public delegate void MethodLoading();