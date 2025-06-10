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
    private List<Resolution> filteredRes;
    private List<string> options;

    private int currentResolutionIdx = 0;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("Fullscreen"))
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
        }

        Load();

        toggle.onValueChanged.AddListener(ChangeScreen);

        CheckResolution();

        // Set to maximum resolution by default
        if (filteredRes.Count > 0)
        {
            int maxResIndex = filteredRes.Count - 1; // Since resolutions are ordered, last one is highest
            ChangeRes(maxResIndex);
            dropdown.value = maxResIndex;
            PlayerPrefs.SetInt("numRes", maxResIndex);
            PlayerPrefs.Save();
        }
    }

    public void ChangeScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
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
        filteredRes = new List<Resolution>();
        dropdown.ClearOptions();

        // Remove duplicate resolutions
        foreach (var res in resolutions)
        {
            if (!filteredRes.Exists(r => r.width == res.width && r.height == res.height))
            {
                filteredRes.Add(res);
            }
        }

        // Order by resolution size
        filteredRes = filteredRes.OrderBy(r => r.width).ThenBy(r => r.height).ToList();

        options = new List<string>();

        currentResolutionIdx = 0;

        for (int i = 0; i < filteredRes.Count; i++)
        {
            string option = $"{filteredRes[i].width}x{filteredRes[i].height}";
            options.Add(option);

            if (filteredRes[i].width == Screen.width && filteredRes[i].height == Screen.height)
            {
                currentResolutionIdx = i;
            }
        }

        dropdown.AddOptions(options);

        int savedIdx = PlayerPrefs.GetInt("numRes", currentResolutionIdx);
        dropdown.value = Mathf.Clamp(savedIdx, 0, filteredRes.Count - 1);
        dropdown.RefreshShownValue();
    }

    public void ChangeRes(int resIdx)
    {
        PlayerPrefs.SetInt("numRes", resIdx);
        PlayerPrefs.Save();

        if (resIdx >= 0 && resIdx < filteredRes.Count)
        {
            Resolution resolution = filteredRes[resIdx];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }
}