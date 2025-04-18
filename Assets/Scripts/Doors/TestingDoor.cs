using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TestingDoor : MonoBehaviour, IInteractableWithButton
{
    [SerializeField] bool thisActivated, hasMultipleOptions;
    [SerializeField] private List<int> thisKey = new List<int>();
    [SerializeField] float detectionRadius;

    private List<InteractableBase> totalKeysActivated = new List<InteractableBase>();
    private SpriteRenderer spriteRenderer;
    private ShadowCaster2D shadowCaster2D;
    private Collider2D col;
    private bool areTwoPlayersOnDoor, stopProcessing = true;

    public static Action RefreshPlayerInventory;

    private IEnumerator Start()
    {
        shadowCaster2D = GetComponent<ShadowCaster2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (thisActivated)
        {
            spriteRenderer.color = Color.green;
            col.isTrigger = true;
            if (shadowCaster2D != null) shadowCaster2D.enabled = false;
        }
        else
        {
            spriteRenderer.color = Color.red;
            col.isTrigger = false;
            if (shadowCaster2D != null) shadowCaster2D.enabled = true;
        }

        yield return null; // Espera un frame para que todos los Interactables hagan su AddToList

        InteractableBase[] interactables = GameObject.FindObjectsByType<InteractableBase>(FindObjectsSortMode.None);
        foreach (var interactable in interactables)
        {
            // Verifica si alguna de las llaves de InteractableBase coincide con esta puerta
            foreach (int key in interactable.GetKey())
            {
                if (thisKey.Contains(key) && !totalKeysActivated.Contains(interactable))
                {
                    totalKeysActivated.Add(interactable);
                }
            }
        }

        stopProcessing = false;
    }

    private void RemoveInteractable(InteractableBase interactable)
    {
        // Asegúrate de que la llave asociada se ha removido correctamente
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
        if (stopProcessing) return;

        int countKeys = 0;

        // Cuenta cuántas llaves activadas hay
        foreach (InteractableBase interactable in totalKeysActivated)
        {
            if (interactable == null) continue;

            KeyBase keyBase = interactable.gameObject.GetComponent<KeyBase>();

            if (keyBase != null && interactable.GetIsActivated())
            {
                countKeys++;
            }
        }

        // Condición para abrir la puerta basada en las llaves activadas
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

        int playerLayerMask = 1 << LayerMask.NameToLayer("Players");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayerMask);

        if (colliders.Length == 2)
            areTwoPlayersOnDoor = true;
        else
            areTwoPlayersOnDoor = false;

        if (colliders.Length != 0)
        {
            if (countKeys != 0)
            {
                int countSameKeys = 0;

                foreach (var collider in colliders)
                {
                    List<KeyBase> keys = collider.gameObject.GetComponent<PlayerInventory>().GetKey();

                    if (keys.Count != 0)
                        foreach (KeyBase key in keys)
                        {
                            foreach (InteractableBase interactable in totalKeysActivated)
                            {
                                if (key.gameObject.GetComponent<InteractableBase>() == interactable)
                                    countSameKeys++;
                            }
                        }
                }

                // Si son las dos llaves y están todas activadas, abre la puerta
                if (areTwoPlayersOnDoor && countKeys == countSameKeys)
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

        if (countKeys == 0) //No tiene ninguna llave activada
            DoorChecker();
    }

    private void DoorChecker()
    {
        bool canDoorBeOpened = true;

        // Si no tiene todas las llaves activadas, no se abre
        foreach (InteractableBase interactable in totalKeysActivated)
        {
            if (!interactable.GetIsActivated())
                canDoorBeOpened = false;

            if (!canDoorBeOpened) break;
        }

        if (canDoorBeOpened && !thisActivated)
            OpenDoor();

        // Si alguna llave no está activada, se cierra la puerta
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

    public void Interacted()
    {
        if (areTwoPlayersOnDoor)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = transform.position + transform.right * 1f;
            GameObject.FindGameObjectWithTag("Kid").transform.position = transform.position + transform.right * 1f;

            //RECARGAR HABILIDAD
        }
    }

    private void OpenDoor()
    {
        spriteRenderer.color = Color.green;
        col.isTrigger = true;
        thisActivated = true;

        if (shadowCaster2D != null) shadowCaster2D.enabled = false;
    }
    private void CloseDoor()
    {
        spriteRenderer.color = Color.red;
        col.isTrigger = false;
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