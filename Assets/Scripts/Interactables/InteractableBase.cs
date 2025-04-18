using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour
{
    [SerializeField] private List<int> keyValue = new List<int>();
    [SerializeField] bool isActivated = false;

    void Start()
    {
        if (gameObject.GetComponent<KeyBase>())
            isActivated = false;
    }

    public List<int> GetKey()
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