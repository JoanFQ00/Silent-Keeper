using System;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{
    [SerializeField] int key;
    [SerializeField] bool activated;
    private SpriteRenderer spriteRenderer;
    private int _activeCollisions = 0;

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Kid")
        {
            activated = true;
            PresureActivated?.Invoke(activated, key);
            spriteRenderer.color = Color.green;

            _activeCollisions++;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Kid")
        {
            _activeCollisions--;

            if (_activeCollisions == 0)
            {
                activated = false;
                PresureActivated?.Invoke(activated, key);
                spriteRenderer.color = new Color(0f, 166f, 255f, 1f);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Kid")
        {
            if (_activeCollisions > 0)
            {
                spriteRenderer.color = Color.green;
            }
        }
    }
}