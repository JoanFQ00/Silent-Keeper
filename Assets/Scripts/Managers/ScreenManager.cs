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
    private List<Resolution> filtredRes;
    private List<string> options;

    private RefreshRate currentRefreshRate;
    private int currentResolutionIdx;

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
        filtredRes = new List<Resolution>();

        dropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRateRatio;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.numerator == currentRefreshRate.numerator &&
                resolutions[i].refreshRateRatio.denominator == currentRefreshRate.denominator)
            {
                filtredRes.Add(resolutions[i]);
            }
        }

        options = new List<string>();
        for (int i = 0; i < filtredRes.Count; i++) 
        {
            string option = filtredRes[i].width + "x" + filtredRes[i].height + " " + filtredRes[i].refreshRateRatio;
            options.Add(option);

            if (filtredRes[i].width == Screen.width && filtredRes[i].height == Screen.height) 
            {
                currentResolutionIdx = i;
            }
        }


        dropdown.AddOptions(options);
        dropdown.value = currentResolutionIdx;
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
