using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int maxLevel = 3;
    public Button[] levelButt;
    private static LevelManager instance;

    void Awake()
    {
        InitButtons();
    }

    private void InitButtons()
    {
        int l_LevelsUnlocked = PlayerPrefs.GetInt("Levels", 3);
        for (int i = 0; i < levelButt.Length; i++)
        {
            levelButt[i].interactable = i <= l_LevelsUnlocked;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            ResetLevels();
        }
    }

    public void UnlockLevel(int nextLevel)
    {
        PlayerPrefs.SetInt("Levels", nextLevel);
    }

    public void ResetLevels()
    {
        PlayerPrefs.SetInt("Levels", 0);
    }
}
