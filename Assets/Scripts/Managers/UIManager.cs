using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
        GameClearPanel.SetActive(false);   
        GameOverPanel.SetActive(false);   
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOver -= ActivateGameOverPanel;
            GameManager.Instance.OnGameClear -= ActivateGameClearPanel;
        }
    }

    private void ActivateGameOverPanel()
    {
        GameOverPanel.SetActive(true);
    }
    private void ActivateGameClearPanel()
    {
        GameClearPanel.SetActive(true);
    }
    public void ActivateSheetPanel()
    {
        SheetPanel.SetActive(true);
    }
    public void DeActivateSheetPanel()
    {
        SheetPanel.SetActive(false);
    }

    public void OnRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif    
    }
}
