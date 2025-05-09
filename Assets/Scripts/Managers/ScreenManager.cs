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

        var currentRefreshRate = Screen.currentResolution.refreshRateRatio;

        for (int i = 0; i < resolutions.Length; i++)
        {
            int width = resolutions[i].width;
            int height = resolutions[i].height;
            var refresh = resolutions[i].refreshRateRatio;

        if (width * 9 != height * 16 || refresh.numerator != currentRefreshRate.numerator || refresh.denominator != currentRefreshRate.denominator)
            continue;

            string option = width + " x " + height + " : " + refresh;
            options.Add(option);

            if (Screen.fullScreen && width == Screen.currentResolution.width && height == Screen.currentResolution.height)
            {
                currentRes = options.Count - 1; // Ajuste del índice
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
