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
    private GameObject optionsPanel, controlsPanel;
    private GameObject buttonPanel;
    private GameObject pausePanel;

    private void Start()
    {
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
        if (controlsPanel == null && FindInCanvasByName("ControlsPanel"))
        {
            controlsPanel = FindInCanvasByName("ControlsPanel");
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
            if (audioSlider.gameObject.name == "Volume_Sl")
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
                sc.LoadGame();
                break;
            case "ExitButt":
                sc.QuitGame();
                break;
            case "MenuButt":
                sc.LoadMenu();
                break;
            case "OptionsButt":
                optionsPanel.SetActive(true);
                buttonPanel.SetActive(false);
                break;
            case "ControlsButt":
                controlsPanel.SetActive(true);
                buttonPanel.SetActive(false);
                break;
            case "BackButt":
                optionsPanel.SetActive(false);
                controlsPanel.SetActive(false);
                buttonPanel.SetActive(true);
                break;
            case "UnpauseButt":
                GameManager.Instance.isPaused = false;
                pausePanel.SetActive(false);
                break;

            default:
                Debug.Log("This Button does nothing");
                break;
        }
    }
}