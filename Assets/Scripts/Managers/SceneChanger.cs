using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;
    public static Action<HudManager.Panels> ChangePan;

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
    private void EnsureAudioManager()
    {
        if (AudioManager.Instance == null)
        {
            GameObject audioManagerPrefab = Resources.Load<GameObject>("Prefabs/AudioManager");
            if (audioManagerPrefab != null)
            {
                Instantiate(audioManagerPrefab);
            }
            else
            {
                Debug.LogWarning("AudioManager prefab not found in Resources/Prefabs.");
            }
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
        GameManager.lastCheckpoint = false;
        Debug.Log("Exit Game");
        Application.Quit();
    }

    public void LoadMenu()
    {
        GameManager.lastCheckpoint = false;
        SceneManager.LoadScene(titleSceneName);
    }

    public void LoadGame()
    {
        EnsureAudioManager();
        GameManager.lastCheckpoint = false;
        SceneManager.LoadScene(level1);
    }
    public void LoadLevel2()
    {
        GameManager.lastCheckpoint = false;
        SceneManager.LoadScene(level2);
    }

    public void LoadLevel3()
    {
        GameManager.lastCheckpoint = false;
        SceneManager.LoadScene(level3);
    }

    public void RestartZone()
    {
        GameManager.lastCheckpoint = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartCheckPointZone()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextZone()
    {
        GameManager.lastCheckpoint = false;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadEnd()
    {
        GameManager.lastCheckpoint = false;
        SceneManager.LoadScene(endingSceneName);
    }
}