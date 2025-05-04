using System;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{
    [SerializeField] bool keepActivated;

    private InteractableBase interactable;
    private SpriteRenderer spriteRenderer;
    private int _activeCollisions = 0;

    void Start()
    {
        interactable = GetComponent<InteractableBase>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (interactable.GetIsActivated())
        {
            spriteRenderer.color = Color.green;
        }
        else
        {
            spriteRenderer.color = spriteRenderer.color = new Color(255f, 255f, 255f, 1f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Kid")
        {
            interactable.SetKeyActivated(true);
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
                if (!keepActivated)
                {
                    interactable.SetKeyActivated(false);
                    spriteRenderer.color = new Color(0f, 166f, 255f, 1f);
                }
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