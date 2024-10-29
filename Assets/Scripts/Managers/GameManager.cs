using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return _instance;
        }
    }
    
    public event Action OnGameOver;
    public event Action OnGameClear;
    
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(_instance != this)
            {
                Destroy(gameObject);
            }
        }
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
