using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DoorBase : MonoBehaviour, IInteractableWithButton
{
    [Header("Door Features Conditions")]
    [Header("")]
    [SerializeField] bool thisActivated;

    [Header("")]
    [SerializeField] bool hasMultipleOptions, isIndependent, keepActivated, isWall;

    [Header("Door Features Values")]
    [SerializeField] private List<int> thisKey = new List<int>();

    [Header("")]
    [SerializeField] float detectionRadius;

    [Header("")]
    [SerializeField] DoorBaseSO doorBaseSO;

    private List<InteractableBase> totalKeysActivated = new List<InteractableBase>();
    private Dictionary<InteractableBase, bool> copyKeyStates = new Dictionary<InteractableBase, bool>();
    private SpriteRenderer spriteRenderer;
    private ShadowCaster2D shadowCaster2D;
    private Collider2D col;
    private bool areTwoPlayersOnDoor, stopProcessing = true;

    public static Action RefreshPlayerInventory;

    private IEnumerator Start()
    {
        stopProcessing = true;

        shadowCaster2D = GetComponent<ShadowCaster2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        spriteRenderer.sprite = doorBaseSO.doorImage;

        if (thisActivated)
        {
            spriteRenderer.color = Color.green;
            if (shadowCaster2D != null) shadowCaster2D.enabled = false;

            if (isWall)
                col.isTrigger = true;
        }
        else
        {
            spriteRenderer.color = Color.red;
            if (shadowCaster2D != null) shadowCaster2D.enabled = true;

            if (isWall)
                col.isTrigger = false;
        }

        yield return null;

        InteractableBase[] interactables = GameObject.FindObjectsByType<InteractableBase>(FindObjectsSortMode.None);
        foreach (var interactable in interactables)
        {
            foreach (int key in interactable.GetKey())
            {
                if (thisKey.Contains(key) && !totalKeysActivated.Contains(interactable))
                {
                    totalKeysActivated.Add(interactable);
                }
            }
        }

        copyKeyStates.Clear();
        foreach (InteractableBase interactable in totalKeysActivated)
            copyKeyStates[interactable] = interactable.GetIsActivated();

        stopProcessing = false;
    }

    private void RemoveInteractable(InteractableBase interactable)
    {
        foreach (int key in interactable.GetKey())
        {
            if (thisKey.Contains(key) && totalKeysActivated.Contains(interactable))
            {
                totalKeysActivated.Remove(interactable);
                interactable.gameObject.GetComponent<KeyBase>().KeyHasBeenUsed();
            }
        }
    }

    private void Update()
    {
        int playerLayerMask = 1 << LayerMask.NameToLayer("Players");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayerMask);

        if (colliders.Length == 2)
            areTwoPlayersOnDoor = true;
        else
            areTwoPlayersOnDoor = false;

        if (stopProcessing) return;

        int countKeys = 0;

        foreach (InteractableBase interactable in totalKeysActivated)
        {
            if (interactable == null) continue;

            KeyBase keyBase = interactable.gameObject.GetComponent<KeyBase>();

            if (keyBase != null && interactable.GetIsActivated())
            {
                countKeys++;
            }
        }

        if (thisActivated && countKeys != 0)
        {
            stopProcessing = true;

            List<InteractableBase> keysToRemove = new List<InteractableBase>();

            foreach (InteractableBase interactable in totalKeysActivated)
                if (interactable.gameObject.GetComponent<KeyBase>() != null)
                    keysToRemove.Add(interactable);

            foreach (InteractableBase interactable in keysToRemove)
                RemoveInteractable(interactable);

            RefreshPlayerInventory?.Invoke();
        }

        if (colliders.Length != 0)
        {
            if (countKeys != 0)
            {
                int countGlobalSameKeys = 0, countSingleSameKeys = 0;

                foreach (var collider in colliders)
                {
                    List<KeyBase> keys = collider.gameObject.GetComponent<PlayerInventory>().GetKey();

                    if (keys.Count != 0)
                        foreach (KeyBase key in keys)
                        {
                            foreach (InteractableBase interactable in totalKeysActivated)
                            {
                                if (key.gameObject.GetComponent<InteractableBase>() == interactable)
                                {
                                    countGlobalSameKeys++;
                                    countSingleSameKeys++;
                                }
                            }
                        }
                }

                if ((areTwoPlayersOnDoor && countKeys == countGlobalSameKeys) || (countKeys == countSingleSameKeys))
                {
                    bool canDoorBeOpened = true;

                    foreach (InteractableBase interactable in totalKeysActivated)
                    {
                        if (!interactable.GetIsActivated())
                            canDoorBeOpened = false;

                        if (!canDoorBeOpened) break;
                    }

                    if (canDoorBeOpened && !thisActivated)
                        OpenDoor();
                }
            }
        }

        if (countKeys == 0)
            DoorChecker();
    }

    private void DoorChecker()
    {
        if (!hasMultipleOptions) //SI O SI SE ABRE CON TODAS A LA VEZ
        {
            bool canDoorBeOpened = true;

            foreach (InteractableBase interactable in totalKeysActivated)
            {
                if (!interactable.GetIsActivated())
                    canDoorBeOpened = false;

                if (!canDoorBeOpened) break;
            }

            if (canDoorBeOpened && !thisActivated)
                OpenDoor();

            foreach (InteractableBase interactable in totalKeysActivated)
            {
                if (!interactable.GetIsActivated())
                    if (thisActivated)
                    {
                        CloseDoor();
                        break;
                    }
            }
        }
        else
        {
            if (hasMultipleOptions && isIndependent) //SOLO NECESITA 1 PARA ACTIVARSE, Y TODAS PARA DESACTIVARSE
            {
                bool canDoorBeOpened = false, canDoorBeClosed = true;

                foreach (InteractableBase interactable in totalKeysActivated)
                {
                    if (interactable.GetIsActivated())
                        canDoorBeOpened = true;

                    if (canDoorBeOpened) break;
                }

                if (canDoorBeOpened && !thisActivated)
                    OpenDoor();

                foreach (InteractableBase interactable in totalKeysActivated)
                {
                    if (interactable.GetIsActivated())
                    {
                        canDoorBeClosed = false;
                        break;
                    }
                }

                if (canDoorBeClosed && thisActivated)
                    CloseDoor();
            }
            else //CADA CAMBIO CAMBIA SU ESTADO
            {
                int numChanges = 0;

                foreach (InteractableBase interactable in totalKeysActivated)
                {
                    bool currentState = interactable.GetIsActivated();

                    if (copyKeyStates.TryGetValue(interactable, out bool previousState))
                    {
                        if (currentState != previousState)
                            numChanges++;
                    }
                    else
                    {
                        numChanges++;
                    }
                }

                if (numChanges > 0)
                {
                    if (numChanges % 2 == 1)
                    {
                        if (thisActivated)
                            CloseDoor();
                        else
                            OpenDoor();
                    }

                    copyKeyStates.Clear();
                    foreach (InteractableBase interactable in totalKeysActivated)
                        copyKeyStates[interactable] = interactable.GetIsActivated();
                }
            }
        }
    }

    private bool hasInteracted = false;
    public void Interacted()
    {
        if (areTwoPlayersOnDoor && thisActivated)
        {
            if (!hasInteracted)
            {
                hasInteracted = true;

                GameObject.FindGameObjectWithTag("Player").transform.position = transform.position + transform.up * 1.2f;
                GameObject.FindGameObjectWithTag("Kid").transform.position = transform.position + transform.up * 1.2f;
            }
            else
            {
                hasInteracted = false;

                GameObject.FindGameObjectWithTag("Player").transform.position = transform.position + transform.up * (-1.5f);
                GameObject.FindGameObjectWithTag("Kid").transform.position = transform.position + transform.up * (-1.5f);
            }

            if (!doorBaseSO.isFinalDoor)
            {
                //RECARGAR HABILIDAD DE SALA
            }
            else
            {
                //RECARGAR HABILIDADES ENTERAS Y LLAMAR MÁQUINA EXPRENDEDORA
            }
        }
    }

    private void OpenDoor()
    {
        if (isWall)
            col.isTrigger = true;

        if (keepActivated)
            stopProcessing = true;

        spriteRenderer.color = Color.green;
        thisActivated = true;

        if (shadowCaster2D != null) shadowCaster2D.enabled = false;
    }
    private void CloseDoor()
    {
        if (isWall)
            col.isTrigger = false;

        spriteRenderer.color = Color.red;
        thisActivated = false;

        if (shadowCaster2D != null) shadowCaster2D.enabled = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void Interacted(GameObject other, GameObject thisObject) { }
}