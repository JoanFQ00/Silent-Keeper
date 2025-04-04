using System;
using System.Threading;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{
    [SerializeField] int key;
    [SerializeField] bool activated;
    SpriteRenderer spriteRenderer;

    public static Action<bool, int> PresureActivated;

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

    void OnTriggerEnter2D(Collider2D other)
    {
        activated = true;
        PresureActivated?.Invoke(activated, key);
        spriteRenderer.color = Color.green;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        activated = false;
        PresureActivated?.Invoke(activated, key);
        spriteRenderer.color = Color.red;
    }
}
