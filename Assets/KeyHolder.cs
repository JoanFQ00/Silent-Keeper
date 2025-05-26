using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyHolder : MonoBehaviour
{
    public GameObject UIElement;
    public GameObject keyText;
    private int keys;

    private void OnEnable()
    {
        DoorBase.RefreshPlayerInventory += HideKeyHolder;
        PlayerInventory.AddKeyUI += AddKey;
    }

    private void OnDisable()
    {
        DoorBase.RefreshPlayerInventory -= HideKeyHolder;
        PlayerInventory.AddKeyUI -= AddKey;
    }

    void Update()
    {
         keyText.GetComponent<TMP_Text>().text = keys.ToString();
    }

     private void AddKey(int amount)
    {
        keys += amount;
        UIElement.SetActive(true); 
    }

    private void HideKeyHolder()
    {
        UIElement.SetActive(false);
        keys = 0;
    }


}
