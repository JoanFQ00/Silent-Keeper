using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;
    public static Action ChangePan;

    private const string titleSceneName = "MainMenu", level1 = "School", level2 = "Circus", level3 = "Void", endingSceneName = "Ending";
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
        GameManager.Instance.ChageZoneState(GameManager.Zone.ZONE_1);
    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene(level2);
        GameManager.Instance.ChageZoneState(GameManager.Zone.ZONE_2);
    }

    public void LoadLevel3() 
    {
        SceneManager.LoadScene(level3);
        GameManager.Instance.ChageZoneState(GameManager.Zone.ZONE_3);
    }

    public void RestartZone()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextZone()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);

        int currentIndex = (int)GameManager.currentZone;
        if (currentIndex < Enum.GetValues(typeof(GameManager.Zone)).Length - 1)
        {
            GameManager.Zone nextZone = (GameManager.Zone)(currentIndex + 1);
            GameManager.Instance.ChageZoneState(nextZone);
        }
    }

    public void LoadEnd()
    {
        SceneManager.LoadScene(endingSceneName);
    }
}