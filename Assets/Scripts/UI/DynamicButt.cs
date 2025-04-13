using UnityEngine;
using UnityEngine.UI;
using System;

public class DynamicButt : MonoBehaviour
{
    private Button button;
    private Slider audioSlider;

    public static Action<Slider> PassMusicSlider;
    public static Action<Slider> PassAudioSlider;

    private void Start()
    {
        if (GetComponent<Button>() != null)
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(HandleButtonClick);
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

            default:
                Debug.Log("This Button does nothing");
                break;
        }
    }
}
