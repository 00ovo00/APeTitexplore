using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event Action OnGameOver;
    public event Action OnGameClear;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        OnGameOver?.Invoke();
    }

    public void GameClear()
    {
        Time.timeScale = 0.0f;
        OnGameClear?.Invoke();
    }
}
