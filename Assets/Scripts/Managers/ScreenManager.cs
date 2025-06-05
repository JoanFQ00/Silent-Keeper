using System.Collections.Generic;
using System.Linq;
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

        int defaultIndex = filtredRes.FindIndex(res => res.width == 1920 && res.height == 1200);
        if (defaultIndex == -1)
        {
            defaultIndex = filtredRes.FindIndex(res => res.width >= 1920 && res.height >= 1200);
        }

        if (defaultIndex != -1)
        {
            ChangeRes(defaultIndex);
            dropdown.value = defaultIndex;
            PlayerPrefs.SetInt("numRes", defaultIndex);
            PlayerPrefs.Save();
        }
        else if (filtredRes.Count > 0)
        {
            // Fallback: usar la primera opción disponible
            ChangeRes(0);
            dropdown.value = 0;
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

        foreach (var res in resolutions)
        {
            float aspectRatio = (float)res.width / res.height;
            if (Mathf.Abs(aspectRatio - 16f / 10f) < 0.01f)
            {
                if (!filtredRes.Exists(r => r.width == res.width && r.height == res.height))
                {
                    filtredRes.Add(res);
                }
            }
        }

        filtredRes = filtredRes.OrderBy(r => r.width).ThenBy(r => r.height).ToList();

        options = new List<string>();

        for (int i = 0; i < filtredRes.Count; i++)
        {
            string option = $"{filtredRes[i].width}x{filtredRes[i].height}";
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
