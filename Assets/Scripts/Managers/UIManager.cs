using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GameObject("UIManager").AddComponent<UIManager>();
            }
            return _instance;
        }
    }
    public GameObject GameOverPanel;
    public GameObject GameClearPanel;
    public GameObject SheetPanel;
    
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            return;
        }
        else
        {
            if(_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnEnable()
    {
        GameManager.Instance.OnGameOver += ActivateGameOverPanel;
        GameManager.Instance.OnGameClear += ActivateGameClearPanel;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOver -= ActivateGameOverPanel;
            GameManager.Instance.OnGameClear -= ActivateGameClearPanel;
        }
    }

    public void ActivateGameOverPanel()
    {
        GameOverPanel.SetActive(true);
    }
    public void ActivateGameClearPanel()
    {
        GameClearPanel.SetActive(true);
    }
}
