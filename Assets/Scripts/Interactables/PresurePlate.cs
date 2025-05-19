using System.Collections.Generic;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{
    [SerializeField] bool keepActivated, desactivatorPlate = false, isBigPlate = false;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Sprite normalPlate, pressedPlate;
    [SerializeField] List<PresurePlate> presurePlates = new List<PresurePlate>();

    private InteractableBase interactable;
    private SpriteRenderer spriteRenderer;
    private HashSet<Collider2D> currentPlayers = new HashSet<Collider2D>();

    void Start()
    {
        interactable = GetComponent<InteractableBase>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (interactable.GetIsActivated())
            spriteRenderer.sprite = pressedPlate;
        else
            spriteRenderer.sprite = normalPlate;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsPlayer(collision)) return;

        currentPlayers.Add(collision);

        if (isBigPlate)
        {
            if (currentPlayers.Count >= 2)
                ActivatePlate();
        }
        else
        {
            ActivatePlate();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsPlayer(collision)) return;

        currentPlayers.Remove(collision);

        if (isBigPlate)
        {
            if (currentPlayers.Count < 2 && !keepActivated)
                DeactivatePlate();
        }
        else
        {
            if (currentPlayers.Count == 0 && !keepActivated)
                DeactivatePlate();
        }
    }

    private void ActivatePlate()
    {
        if (desactivatorPlate)
        {
            foreach (PresurePlate plate in presurePlates)
            {
                plate.GetComponent<InteractableBase>().SetKeyActivated(false);
                plate.SetToNormalSprite();
            }
        }
        else
        {
            interactable.SetKeyActivated(true);
        }

        spriteRenderer.sprite = pressedPlate;
    }

    private void DeactivatePlate()
    {
        interactable.SetKeyActivated(false);
        spriteRenderer.sprite = normalPlate;
    }

    private bool IsPlayer(Collider2D col)
    {
        return ((1 << col.gameObject.layer) & playerLayer) != 0;
    }

    public void SetToNormalSprite()
    {
        spriteRenderer.sprite = normalPlate;
    }
}