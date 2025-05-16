using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;

    private const string titleSceneName = "MainMenu", level1 = "School", level2 = "Circus",level3 = "", endingSceneName = "Ending";
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
    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            NextZone();
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
        SceneManager.LoadScene(level1);
    }
    public void LoadLevel2() 
    {
        SceneManager.LoadScene(level2);
    }

    public void LoadLevel3() 
    {
        SceneManager.LoadScene(level3);
    }

    public void RestartZone()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextZone()
    {
        SceneManager.LoadScene(level2);
    }

    public void LoadEnd()
    {
        SceneManager.LoadScene(endingSceneName);
    }
}