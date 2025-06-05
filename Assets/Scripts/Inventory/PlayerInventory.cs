using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerInventory : MonoBehaviour
{
    private List<KeyBase> collectedKeys;

    public static Action KeyHasBeenUsed;
    public static Action<int, KeyBase.KeyType> AddKeyUI, RemoveKeyUI;

    private void OnEnable()
    {
        DoorBase.RefreshPlayerInventory += RefreshKeys;
    }

    private void OnDisable()
    {
        DoorBase.RefreshPlayerInventory -= RefreshKeys;
    }

    void Start()
    {
        collectedKeys = new List<KeyBase>();
    }

    public List<KeyBase> GetKey()
    {
        return collectedKeys.FindAll(k => k != null && k.gameObject.activeSelf);
    }

    public void RefreshKeys(KeyBase key)
    {
        collectedKeys.Remove(key);
        RemoveKeyUI?.Invoke(1, key.keyType);
        key.gameObject.SetActive(false);
    }

    public void OnKeyObtained(KeyBase keyBase)
    {
        collectedKeys.Add(keyBase);
        AddKeyUI?.Invoke(1, keyBase.keyType);
    }
}