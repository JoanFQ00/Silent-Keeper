using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static KeyBase;

public class HudManager : MonoBehaviour
{
    public GameObject keyText;
    private int keys;
    public Image keyIm1, keyIm2, ciIm, rcIm, usIm;
    public Color blueColor = new Color(30f / 255f, 54f / 255f, 1f, 1f);
    public Color orangeColor = new Color(1f, 98f / 255f, 0f, 1f);
    public GameObject schoolPan;
    public GameObject circusPan;
    public GameObject voidPan;
    public enum Panels { schoolPan, circusPan, voidPanel}
    public Panels currrentPanel;

    private void OnEnable()
    {
        PlayerInventory.AddKeyUI += AddKey;
        //PlayerInventory.RemoveKeyUI += RemoveKey;
        SceneChanger.ChangePan += ChangePanel;
    }

    private void OnDisable()
    {
        PlayerInventory.AddKeyUI -= AddKey;
        //PlayerInventory.RemoveKeyUI -= RemoveKey;
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

        if (voidPan == null) 
        {
            voidPan = FindInActiveObjectByName("voidPanel");
        }

        if (SceneManager.GetActiveScene().name == "Circus")
        {
            ChangePanel(Panels.circusPan);
        }
        else if (SceneManager.GetActiveScene().name == "Void")
        {
            ChangePanel(Panels.voidPanel);
        }
        else if (SceneManager.GetActiveScene().name == "School")
        {
            ChangePanel(Panels.schoolPan);
        }
    }

    void Update()
    {
        if (keyText != null)
        {
            keyText.GetComponent<TMP_Text>().text = keys.ToString() + "/3";
        }
    }

    private void ChangePanel(Panels panel)
    {
        if (panel == Panels.circusPan)
        {
            currrentPanel = Panels.circusPan;
            schoolPan.SetActive(false);
            //circusPan.SetActive(true);
            //voidPan.SetActive(false);
        }
        else if (panel == Panels.circusPan)
        {
            currrentPanel = Panels.schoolPan;
            schoolPan.SetActive(true);
            //circusPan.SetActive(false);
            //voidPan.SetActive(false);
        }
        else if (panel == Panels.voidPanel) 
        {
            currrentPanel = Panels.voidPanel;
            schoolPan.SetActive(false);
            //circusPan.SetActive(false);
            //voidPan.SetActive(true);
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

    //private void RemoveKey(int amount, KeyBase.KeyType keyType) 
    //{
    //    switch (keyType)
    //    {
    //        case KeyBase.KeyType.blue:
    //            keyIm1.color = Color.black;
    //            break;
    //        case KeyBase.KeyType.orange:
    //            keyIm2.color = Color.black;
    //            break;
    //        }
    //    }
    
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