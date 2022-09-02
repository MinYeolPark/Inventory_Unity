using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private MethodLoading methodStart, methodLoaded;
    [SerializeField] private float loadingDt = 0f;
    [SerializeField] private float loadingSpeed = 200f;

    private void Update()
    {
        if(loadingDt !=0f)
        {
            loadingDt += loadingSpeed * Time.deltaTime;
        }
    }
    public void setLoading(GameState state, MethodLoading start, MethodLoading end)
    {

    }
    public void loading()
    {

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