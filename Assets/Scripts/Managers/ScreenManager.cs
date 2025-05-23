using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private Toggle toggle; 
    public TMP_Dropdown dropdown; 

    private Resolution[] resolutions;
    private List<Resolution> filtredRes;
    private List<string> options;

    private RefreshRate currentRefreshRate;
    private int currentResolutionIdx; 

    void Awake()
    {
        if (!PlayerPrefs.HasKey("Fullscreen"))
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
        }

        Load();

        toggle.isOn = Screen.fullScreen;

        CheckResolution();

        if (!PlayerPrefs.HasKey("numRes"))
        {
            int index1920x1200 = filtredRes.FindIndex(res => res.width == 1920 && res.height == 1200);

            if (index1920x1200 != -1)
            {
                ChangeRes(index1920x1200);
                dropdown.value = index1920x1200;
                PlayerPrefs.SetInt("numRes", index1920x1200);
                PlayerPrefs.Save();
            }
  
        }
    }


    public void ChangeScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
        toggle.isOn = fullScreen;
        Save();
    }

    private void Load()
    {
        toggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1;
    }

    private void Save()
    {
        PlayerPrefs.SetInt("Fullscreen", toggle.isOn ? 1 : 0);
        PlayerPrefs.Save(); 
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
            string option = $"{filtredRes[i].width}x{filtredRes[i].height} @ {filtredRes[i].refreshRateRatio.value:0}Hz";
            options.Add(option);

            if (filtredRes[i].width == Screen.width && filtredRes[i].height == Screen.height)
            {
                currentResolutionIdx = i;
            }
        }

        dropdown.AddOptions(options); 

        int savedIdx = PlayerPrefs.GetInt("numRes", currentResolutionIdx);

        dropdown.value = Mathf.Clamp(savedIdx, 0, filtredRes.Count - 1);
        dropdown.RefreshShownValue();
    }

    public void ChangeRes(int resIndx)
    {
        PlayerPrefs.SetInt("numRes", resIndx);
        PlayerPrefs.Save(); 

        if (resIndx >= 0 && resIndx < filtredRes.Count)
        {
            Resolution resolution = filtredRes[resIndx];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }
}
