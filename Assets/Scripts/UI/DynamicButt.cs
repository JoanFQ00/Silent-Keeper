using UnityEngine;
using UnityEngine.UI;
using System;

public class DynamicButt : MonoBehaviour
{
    public static Action<Slider> PassMusicSlider;
    public static Action<Slider> PassAudioSlider;

    public Canvas canvas;

    private Button button;
    private Slider audioSlider;
    private GameObject optionsPanel;
    private GameObject buttonPanel;
    private GameObject pausePanel;

    private void Start()
    {
        if (canvas != null)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = 9999;
        }

        if (GetComponent<Button>() != null)
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(HandleButtonClick);
        }
        if (canvas == null && GameObject.Find("Menu"))
        {
            canvas = GameObject.Find("Menu").GetComponent<Canvas>();
        }
        if (optionsPanel == null && FindInCanvasByName("OptionsPanel"))
        {
            optionsPanel = FindInCanvasByName("OptionsPanel");
        }
        if (buttonPanel == null && FindInCanvasByName("ButtonPanel"))
        {
            buttonPanel = FindInCanvasByName("ButtonPanel");
        }
        if (pausePanel == null && FindInCanvasByName("PauseMenu"))
        {
            pausePanel = FindInCanvasByName("PauseMenu");
        }
    }

    private void OnEnable()
    {
        if (GetComponent<Slider>() != null)
        {
            audioSlider = GetComponent<Slider>();
            if (audioSlider.gameObject.name == "VolumeM_Sl")
            {
                PassMusicSlider?.Invoke(audioSlider);
            }
            if (audioSlider.gameObject.name == "VolumeS_Sl")
            {
                PassAudioSlider?.Invoke(audioSlider);
            }
        }
    }

    private GameObject FindInCanvasByName(string name)
    {
        Transform[] children = canvas.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    private void HandleButtonClick()
    {
        SceneChanger sc = FindAnyObjectByType<SceneChanger>();

        switch (gameObject.name)
        {
            case "StartButt":
                sc.NextZone();
                break;
            case "ExitButt":
                sc.QuitGame();
                break;
            case "MenuButt":
                sc.LoadMenu();
                Time.timeScale = 1.0f;
                break;
            case "OptionsButt":
                optionsPanel.SetActive(true);
                buttonPanel.SetActive(false);
                break;
            case "ControlsButt":
                buttonPanel.SetActive(false);
                break;
            case "BackButt":
                optionsPanel.SetActive(false);
                buttonPanel.SetActive(true);
                break;
            case "UnpauseButt":
                GameManager.Instance.Pause();
                break;
            case "Level1":
                sc.LoadGame();
                break;
            case "Level2":
                sc.LoadLevel2();
                break;
            case "Level3":
                sc.LoadLevel3();
                break;
            case "ResetButt":
                sc.RestartZone();
                break;

            default:
                break;
        }
    }
}