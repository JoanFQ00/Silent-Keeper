using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerInventory : MonoBehaviour
{
    private List<KeyBase> collectedKeys;

    public static Action KeyHasBeenUsed;

    private void OnEnable()
    {
        TestingDoor.RefreshPlayerInventory += RefreshKeys;
    }

    private void OnDisable()
    {
        TestingDoor.RefreshPlayerInventory -= RefreshKeys;

    }

    void Start()
    {
        collectedKeys = new List<KeyBase>();
    }

    public List<KeyBase> GetKey()
    {
        if (collectedKeys.Count == 0)
            return new List<KeyBase>();
        return collectedKeys;
    }

    public void RefreshKeys()
    {
        collectedKeys.RemoveAll(k => k == null);

        Transform followTarget = transform;

        for (int i = 0; i < collectedKeys.Count; i++)
        {
            collectedKeys[i].SetFollowTarget(followTarget);
            followTarget = collectedKeys[i].transform;
        }

        KeyHasBeenUsed?.Invoke();
    }

    public void OnKeyObtained(KeyBase keyBase)
    {
        Transform followTarget = null;

        if (collectedKeys.Count == 0)
            followTarget = transform;
        else
            followTarget = collectedKeys[collectedKeys.Count - 1].transform;

        keyBase.SetFollowTarget(followTarget);
        collectedKeys.Add(keyBase);
    }
}