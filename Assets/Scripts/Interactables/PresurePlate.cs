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
            spriteRenderer.color = spriteRenderer.color = new Color(0f, 166f, 255f, 1f);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        activated = false;
        PresureActivated?.Invoke(activated, key);
        spriteRenderer.color = new Color(0f, 166f, 255f, 1f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        activated = true;
        PresureActivated?.Invoke(activated, key);
        spriteRenderer.color = Color.green;
    }
}
