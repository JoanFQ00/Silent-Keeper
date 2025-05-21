using System.Collections.Generic;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{
    [SerializeField] bool keepActivated = false, desactivatorPlate = false, isBigPlate = false, onlyBox = false;
    [SerializeField] LayerMask playerLayer, enemyLayer, boxLayer;
    [SerializeField] Sprite normalPlate, pressedPlate;
    [SerializeField] List<PresurePlate> presurePlates = new List<PresurePlate>();
    [SerializeField] private float boxSnapDuration = 0.3f;

    private InteractableBase interactable;
    private SpriteRenderer spriteRenderer;
    private HashSet<Collider2D> currentPlayers = new HashSet<Collider2D>();

    private class BoxTimerInfo
    {
        public float timer = 0f;
        public Transform boxTransform;
        public Vector3 targetPosition;
        public bool isMoving = false;
    }

    private Dictionary<Collider2D, BoxTimerInfo> boxTimers = new Dictionary<Collider2D, BoxTimerInfo>();

    void Start()
    {
        interactable = GetComponent<InteractableBase>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (interactable.GetIsActivated())
            spriteRenderer.sprite = pressedPlate;
        else
            spriteRenderer.sprite = normalPlate;
    }

    void Update()
    {
        List<Collider2D> boxesToActivate = new List<Collider2D>();

        foreach (var kvp in boxTimers)
        {
            Collider2D boxCollider = kvp.Key;
            BoxTimerInfo info = kvp.Value;

            if (!info.isMoving)
            {
                info.timer += Time.deltaTime;

                if (info.timer >= 2f)
                {
                    Vector3 currentPos = info.boxTransform.position;
                    float gridSize = 0.5f;
                    info.targetPosition = new Vector3(
                        Mathf.Round(currentPos.x / gridSize) * gridSize,
                        Mathf.Round(currentPos.y / gridSize) * gridSize,
                        currentPos.z
                    );
                    info.isMoving = true;
                }
            }
            else
            {
                info.boxTransform.position = Vector3.MoveTowards(
                    info.boxTransform.position,
                    info.targetPosition,
                    (1 / boxSnapDuration) * Time.deltaTime
                );

                if (Vector3.Distance(info.boxTransform.position, info.targetPosition) < 0.01f)
                {
                    info.boxTransform.position = info.targetPosition;
                    boxesToActivate.Add(boxCollider);
                }
            }
        }

        foreach (var box in boxesToActivate)
        {
            ActivatePlate();
            boxTimers.Remove(box);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (onlyBox)
        {
            if (!IsBox(collision)) return;

            if (!boxTimers.ContainsKey(collision))
            {
                boxTimers[collision] = new BoxTimerInfo
                {
                    boxTransform = collision.transform
                };
            }

            return;
        }
        else
        {
            if (!IsPlayer(collision)) return;
        }

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
        if (onlyBox)
        {
            if (!IsBox(collision)) return;

            if (boxTimers.ContainsKey(collision))
                boxTimers.Remove(collision);

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
