using System.Collections.Generic;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{
    [SerializeField] bool keepActivated = false, desactivatorPlate = false, isBigPlate = false, onlyBox = false, hasToActivateObjects = false, hasToDisableObjects = false;
    [SerializeField] LayerMask playerLayer, enemyLayer, boxLayer;
    [SerializeField] Sprite normalPlate, pressedPlate, mediumPlate;
    [SerializeField] List<PresurePlate> presurePlates = new List<PresurePlate>();
    [SerializeField] private float boxSnapDuration = 0.3f;
    [SerializeField] List<GameObject> objectsToEnabe = new List<GameObject>();

    private InteractableBase interactable;
    private SpriteRenderer spriteRenderer;
    private HashSet<Collider2D> currentPlayers = new HashSet<Collider2D>();
    private bool objectsEnabled = false;

    private class BoxTimerInfo
    {
        public float timer = 0f;
        public Transform boxTransform;
        public Vector3 targetPosition;
        public bool isMoving = false;
    }

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
        if (onlyBox)
        {
            if (!IsBox(collision)) return;

            AudioManager.Instance.PlaySFX("pressurePlate");
            ActivatePlate();
            return;
        }
        else
        {
            if (!IsPlayer(collision)) return;
        }

        currentPlayers.Add(collision);

        if (currentPlayers.Count == 1)
            AudioManager.Instance.PlaySFX("pressurePlate");

        if (isBigPlate)
        {
            if (currentPlayers.Count >= 2)
            {
                ActivatePlate();
                AudioManager.Instance.PlaySFX("pressurePlate");
            }
            else if (currentPlayers.Count == 1)
                MediumPlate();
        }
        else
        {
            ActivatePlate();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (onlyBox)
        {
            if (!IsBox(collision)) return;

            if (!keepActivated)
                DeactivatePlate();

            return;
        }
        else
        {
            if (!IsPlayer(collision)) return;
        }

        currentPlayers.Remove(collision);

        if (isBigPlate)
        {
            if (!keepActivated)
            {
                if (currentPlayers.Count == 1)
                    MediumPlate();
                else if (currentPlayers.Count == 0)
                    DeactivatePlate();
            }
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
            AudioManager.Instance.PlaySFX("error");

            foreach (PresurePlate plate in presurePlates)
            {
                plate.GetComponent<InteractableBase>().SetKeyActivated(false);
                plate.SetToNormalSprite();
            }
        }
        else
        {
            if ((onlyBox || (hasToActivateObjects && !hasToDisableObjects)) && !objectsEnabled)
            {
                if (objectsToEnabe != null)
                    foreach (GameObject element in objectsToEnabe)
                        element.SetActive(true);

                objectsEnabled = true;
            }
            else if ((onlyBox || (hasToActivateObjects && hasToDisableObjects)) && !objectsEnabled)
            {
                if (objectsToEnabe != null)
                    foreach (GameObject element in objectsToEnabe)
                        element.SetActive(false);

                objectsEnabled = true;
            }

            interactable.SetKeyActivated(true);
        }

        spriteRenderer.sprite = pressedPlate;
    }

    private void MediumPlate()
    {
        interactable.SetKeyActivated(false);
        spriteRenderer.sprite = mediumPlate;
    }

    private void DeactivatePlate()
    {
        interactable.SetKeyActivated(false);
        spriteRenderer.sprite = normalPlate;
    }

    private bool IsPlayer(Collider2D col)
    {
        int colLayerMask = 1 << col.gameObject.layer;
        return (colLayerMask & playerLayer) != 0 || (colLayerMask & enemyLayer) != 0;
    }

    private bool IsBox(Collider2D col)
    {
        int colLayerMask = 1 << col.gameObject.layer;
        return (colLayerMask & boxLayer) != 0;
    }

    public void SetToNormalSprite()
    {
        spriteRenderer.sprite = normalPlate;
    }
}