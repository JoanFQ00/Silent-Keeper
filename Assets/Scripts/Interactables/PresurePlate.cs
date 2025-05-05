using System;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{
    [SerializeField] bool keepActivated;
    [SerializeField] LayerMask playerLayer;

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
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            interactable.SetKeyActivated(true);
            spriteRenderer.color = Color.green;//Cambiar

            _activeCollisions++;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            _activeCollisions--;

            if (_activeCollisions == 0)
            {
                if (!keepActivated)
                {
                    interactable.SetKeyActivated(false);
                    spriteRenderer.color = new Color(0f, 166f, 255f, 1f);//Cambiar
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            if (_activeCollisions > 0)
            {
                spriteRenderer.color = Color.green;
            }
        }
    }
}