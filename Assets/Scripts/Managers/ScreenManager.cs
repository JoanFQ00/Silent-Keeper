using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    private static ScreenManager instance;
    [SerializeField] private Toggle toggle;
    public TMP_Dropdown dropdown;
    Resolution[] resolutions;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("Fullscreen"))
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
            Load();
        }
        else
        {
            Load();
        }

        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
        CheckResolution();
    }

    public void ChangeScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
        toggle.isOn = fullScreen;
        Save();
    }

    private void Load()
    {
        if (PlayerPrefs.GetInt("Fullscreen") == 1)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }

    private void Save()
    {
        if (toggle.isOn == true)
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Fullscreen", 0);
        }
    }

    public void CheckResolution()
    {
        resolutions = Screen.resolutions;
        dropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentRes = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " : " + resolutions[i].refreshRateRatio;
            options.Add(option);

            if (Screen.fullScreen && (resolutions[i].width == Screen.currentResolution.width) &&
                (resolutions[i].height == Screen.currentResolution.height))
            {
                currentRes = i;
            }
        }

        dropdown.AddOptions(options);
        dropdown.value = currentRes;
        dropdown.RefreshShownValue();

        dropdown.value = PlayerPrefs.GetInt("numRes", 0);
    }

    public void ChangeRes(int resIndx)
    {
        PlayerPrefs.SetInt("numRes", dropdown.value);

        Resolution resolution = resolutions[resIndx];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
