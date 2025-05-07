using System.Collections.Generic;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{
    [SerializeField] bool keepActivated, desactivatorPlate = false;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Sprite normalPlate, pressedPlate;
    [SerializeField] List<PresurePlate> presurePlates = new List<PresurePlate>();

    private InteractableBase interactable;
    private SpriteRenderer spriteRenderer;
    private int _activeCollisions = 0;

    void Start()
    {
        interactable = GetComponent<InteractableBase>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (interactable.GetIsActivated())
        {
            spriteRenderer.sprite = pressedPlate;
        }
        else
        {
            spriteRenderer.sprite = normalPlate;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            if (desactivatorPlate)
                foreach (PresurePlate plate in presurePlates)
                {
                    plate.GetComponent<InteractableBase>().SetKeyActivated(false);
                    plate.SetToNormalSprite();
                }
            else
                interactable.SetKeyActivated(true);

            spriteRenderer.sprite = pressedPlate;

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
                    spriteRenderer.sprite = normalPlate;
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
                spriteRenderer.sprite = pressedPlate;
            }
        }
    }

    public void SetToNormalSprite()
    {
        spriteRenderer.sprite = normalPlate;
    }
}