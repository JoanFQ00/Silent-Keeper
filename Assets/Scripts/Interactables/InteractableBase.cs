using System;
using UnityEngine;

public class InteractableBase : MonoBehaviour
{
    [SerializeField] int keyValue = 0;
    [SerializeField] bool isActivated = false;

    void Start()
    {
        if (gameObject.GetComponent<KeyBase>())
            isActivated = false;
    }

    public int GetKey()
    {
        return keyValue;
    }

    public bool GetIsActivated()
    {
        return isActivated;
    }

    public void SetKeyActivated(bool value)
    {
        isActivated = value;
    }
}