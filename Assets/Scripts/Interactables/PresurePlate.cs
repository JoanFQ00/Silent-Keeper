using System.Collections.Generic;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{
    [SerializeField] bool keepActivated = false, desactivatorPlate = false, isBigPlate = false, onlyBox = false, hasToActivateObjects = false, hasToDisableObjects = false;
    [SerializeField] LayerMask playerLayer, enemyLayer, boxLayer;
    [SerializeField] Sprite normalPlate, pressedPlate, mediumPlate;
    [SerializeField] List<PresurePlate> presurePlates = new List<PresurePlate>();
    [SerializeField] private float boxSnapDuration = 0.3f;
    [SerializeField] private float boxActivationTime = 2f; // <- NUEVA VARIABLE
    [SerializeField] List<GameObject> objectsToEnabe = new List<GameObject>();

    private InteractableBase interactable;
    private SpriteRenderer spriteRenderer;
    private HashSet<Collider2D> currentPlayers = new HashSet<Collider2D>();
    private bool objectsEnabled = false;

    private Dictionary<Collider2D, float> boxTimers = new Dictionary<Collider2D, float>();
    private bool boxPlateActivated = false;

    void Start()
    {
        interactable = GetComponent<InteractableBase>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = interactable.GetIsActivated() ? pressedPlate : normalPlate;
    }

    void Update()
    {
        if (onlyBox && !boxPlateActivated && boxTimers.Count > 0)
        {
            var keys = new List<Collider2D>(boxTimers.Keys);
            foreach (var box in keys)
            {
                if (box == null)
                {
                    boxTimers.Remove(box);
                    continue;
                }

                boxTimers[box] += Time.deltaTime;

                if (boxTimers[box] >= boxActivationTime)
                {
                    boxPlateActivated = true;
                    ActivatePlate();
                    break;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (onlyBox)
        {
            if (!IsBox(collision)) return;

            if (!boxTimers.ContainsKey(collision))
                boxTimers.Add(collision, 0f);

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

            if (boxTimers.ContainsKey(collision))
                boxTimers.Remove(collision);

            if (!keepActivated)
            {
                boxPlateActivated = false;
                DeactivatePlate();
            }

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
        if (interactable.GetIsActivated()) return;

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
                foreach (GameObject element in objectsToEnabe)
                    element.SetActive(true);
                objectsEnabled = true;
            }
            else if ((onlyBox || (hasToActivateObjects && hasToDisableObjects)) && !objectsEnabled)
            {
                foreach (GameObject element in objectsToEnabe)
                    element.SetActive(false);
                objectsEnabled = true;
            }

            interactable.SetKeyActivated(true);
        }

        spriteRenderer.sprite = pressedPlate;
        AudioManager.Instance.PlaySFX("pressurePlate");
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