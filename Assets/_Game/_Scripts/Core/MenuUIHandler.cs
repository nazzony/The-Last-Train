using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject SettingsPanel;
    private bool isActive = false;

    // Buttons Handling
    public void ReturnToMainMenu()
    {
        if(Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
            GameIsPaused = false;
        }
        SceneManager.LoadScene((int)Scenes.MainMenu);
    }
    public void StartNew()
    {
        SceneManager.LoadScene((int)Scenes.Scene1);
    }
    public void OpenSettings()
    {
        SettingsPanel.SetActive(true);
        isActive = true;
    }
    public void CloseSettings()
    {
        SettingsPanel.SetActive(false);
        isActive = false;
    }
    
    public void Exit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
    public enum Scenes
    {
        MainMenu = 0,
        Settings = 1,
        Scene1 = 2,
        PauseMenu = 3,
    }

    void Update()
    {

    }
    // Pause Menu Handling

    public static bool GameIsPaused = false;
    public GameObject PauseMenuUi;

    public void Pause()
    {
        PauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void Resume()
    {
        PauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
}
