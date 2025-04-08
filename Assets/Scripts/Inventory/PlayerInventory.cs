using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //Más adelante hacer tabla
    private int keyValue;

    void Start()
    {
        keyValue = 0;
    }

    public int GetKeyValue()
    {
        return keyValue;
    }

    public void OnKeyObtained(int key)
    {
        keyValue = key;
    }
}