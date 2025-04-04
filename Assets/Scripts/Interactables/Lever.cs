using System;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] int key;
    [SerializeField] bool activated;
    public static Action<bool, int> LeverActivated;

    public void Interacted() 
    {
        activated = !activated;
        LeverActivated?.Invoke(activated, key);

        Debug.Log(activated + " " + key);
    }
}
