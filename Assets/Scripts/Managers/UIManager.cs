using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public GameObject GameOverPanel;
    public GameObject GameClearPanel;
    public GameObject SheetPanel;
    
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
