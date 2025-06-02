using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    public GameObject keyText;
    private int keys;
    public Image keyIm1, keyIm2, ciIm, rcIm, usIm;
    public Color blueColor = new Color(30f / 255f, 54f / 255f, 1f, 1f);
    public Color orangeColor = new Color(1f, 98f / 255f, 0f, 1f);
    public GameObject schoolPan;
    public GameObject circusPan;
    public enum Panels { schoolPan, circusPan}
    public Panels currrentPanel;

    private void OnEnable()
    {
        PlayerInventory.AddKeyUI += AddKey;
        SceneChanger.ChangePan += ChangePanel;
    }

    private void OnDisable()
    {
        PlayerInventory.AddKeyUI -= AddKey;
        SceneChanger.ChangePan -= ChangePanel;
    }

    void Awake()
    {
        if (schoolPan == null)
        {
            schoolPan = FindInActiveObjectByName("TabPanel");
        }

        if (circusPan == null)
        {
            circusPan = FindInActiveObjectByName("CircusTabPanel");
        }

        if (SceneManager.GetActiveScene().name == "Circus")
        {
            ChangePanel();
        }
        
    }

    void Update()
    {
        if (keyText != null)
        {
            keyText.GetComponent<TMP_Text>().text = keys.ToString() + "/3";
        }
    }

    private void ChangePanel()
    {
        if (currrentPanel == Panels.schoolPan)
        {
            currrentPanel = Panels.circusPan;
            schoolPan.SetActive(false);
            circusPan.SetActive(true);
        }
        else
        {
            currrentPanel = Panels.schoolPan;
            schoolPan.SetActive(true);
            circusPan.SetActive(false);
        }
    }

    private void AddKey(int amount, KeyBase.KeyType keyType)
    {
        switch (keyType)
        {
            case KeyBase.KeyType.blue:
                keyIm1.color = blueColor;
                break;
            case KeyBase.KeyType.orange:
                keyIm2.color = orangeColor;
                break;
            case KeyBase.KeyType.ci:
                keys += amount;
                ciIm.color = Color.white;
                break;
            case KeyBase.KeyType.rc:
                keys += amount;
                rcIm.color = Color.white;
                break;
             case KeyBase.KeyType.us:
                keys += amount;
                usIm.color = Color.white;
                break;
        }
    }
    
    private GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in objs)
        {
            if (t.hideFlags == HideFlags.None && t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}