using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;

    private const string titleSceneName = "MainMenu", gameplaySceneName = "Demo", endingSceneName = "Ending";
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnStart()
    {
        LoadGame();
    }

    public void QuitGame()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(titleSceneName);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void LoadEnd()
    {
        SceneManager.LoadScene(endingSceneName);
    }
}