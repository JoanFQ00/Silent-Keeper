using System;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] int key;
    [SerializeField] bool activated;

    private SpriteRenderer spriteRenderer;

    public static Action<bool, int> LeverActivated;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();   
        if (activated)
        {
            spriteRenderer.color = Color.green;
        } 
        else 
        {
            spriteRenderer.color = Color.red;
        }
    }

    public void Interacted() 
    {
        activated = !activated;
        LeverActivated?.Invoke(activated, key);

        if (activated)
        {
            spriteRenderer.color = Color.green;
        } 
        else 
        {
            spriteRenderer.color = Color.red;
        }
    }
}