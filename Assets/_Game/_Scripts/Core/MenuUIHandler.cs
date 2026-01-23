using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject SettingsPanel;

    // Buttons Handling
    public void ReturnToMainMenu()
    {
        if(Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
            GameIsPaused = false;
        }
        SceneFader.instance.LoadScene((int)Scenes.MainMenu);
    }
    public void StartNew()
    {
        GameData.TargetDoorId = -1;
        SceneFader.instance.LoadScene((int)Scenes.Scene1);
    }
    public void OpenSettings()
    {
        SettingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        SettingsPanel.SetActive(false);
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
        Scene1 = 1,
        Scene2 = 2,
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
