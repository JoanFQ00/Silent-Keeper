using System;
using UnityEngine;
using TMPro;

public class DisplayMode : MonoBehaviour
{
    public static Action<string> OnChangeTxt;
    [SerializeField] private TMP_Text text;

    void Start()
    {
        text.text = "Current Mode: ";
    }

    private void OnEnable()
    {
        MonsterMove.OnChangeTxt += ChangeTxt;
    }

    private void OnDisable()
    {
        MonsterMove.OnChangeTxt -= ChangeTxt;
    }

    private void ChangeTxt(string newTxt) 
    {
        text.text = "Modo Actual: " + newTxt;
    }
}
