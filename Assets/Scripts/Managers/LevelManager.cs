using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private Dictionary<int, bool> Levels;
    public int maxLevel = 3;
    public Button[] levelButt;
    private static LevelManager instance;

    void Awake()
    {
        Levels = new Dictionary<int, bool>
        {
            { 1, true }
        };
        
        for (int i = 2; i < maxLevel; i++) 
        {
           Levels.Add(i, false);
        }

        if (!PlayerPrefs.HasKey("Levels"))
        {
            PlayerPrefs.SetInt("Levels", 1);
            Load();
            ChangeColor();
        }
        else
        {
            Load();
            ChangeColor();
        }
    }

    private void ChangeColor() 
    {
        for (int i = 0; i <levelButt.Length; i++ ) 
        {
            int level = i + 1;
                if (Levels.ContainsKey(level))
                {
                    bool unlocked = Levels[level];
                    Button btn = levelButt[i];
                    btn.interactable = unlocked;

                    ColorBlock colors = btn.colors;
                    if (unlocked)
                    {
                        colors.normalColor = Color.white;
                    }
                    else
                    {
                        colors.normalColor = Color.gray;
                    }
                btn.colors = colors;
            }
        }
    }

     private void Load() 
     {
        int unlockedLevels = PlayerPrefs.GetInt("Levels");

        for (int i = 1; i <= unlockedLevels; i++) 
        {
            if (Levels.ContainsKey(i))
                Levels[i] = true;
        }
     }
}
