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
    [SerializeField] bool hasMultipleOptions, isIndependent, keepActivated, isMobile;
    [SerializeField] private TPAxis detectionAxis = TPAxis.UpDown;

    [Header("Door Features Values")]
    [SerializeField] private int doorKey;

    [SerializeField] private int doorKeyTP;

    [Header("")]
    [SerializeField] DoorBaseSO doorBaseSO;

    private List<InteractableBase> totalKeysActivated = new List<InteractableBase>();
    private Dictionary<InteractableBase, bool> copyKeyStates = new Dictionary<InteractableBase, bool>();
    private SpriteRenderer spriteRenderer;
    private ShadowCaster2D shadowCaster2D;
    private Collider2D col;
    private GameObject teleportDestination;
    private bool areTwoPlayersOnDoor, stopProcessing = true;
    private float detectionRadius;
    private DoorBase doorTP;

    public static Action RefreshPlayerInventory, OnZoneEnded;
    public static Action<int, Transform> OnStartCamTransition;

    private void OnEnable()
    {
        TransitionCamAnimation.OnChangedRoom += TeleportPlayers;
    }

    private void OnDisable()
    {
        TransitionCamAnimation.OnChangedRoom -= TeleportPlayers;
    }

    private enum TPAxis
    {
        UpDown,
        RightLeft
    }

    private IEnumerator Start()
    {
        stopProcessing = true;

        shadowCaster2D = GetComponentInChildren<ShadowCaster2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        detectionRadius = GetComponent<InteractTxt>() != null ? GetComponent<InteractTxt>().GetInteractableArea() : 1.0f;

        if (doorBaseSO != null)
            spriteRenderer.sprite = doorBaseSO.doorImage;

        if (thisActivated)
        {
            spriteRenderer.sprite = doorBaseSO.openDoorImage;
            if (shadowCaster2D != null) shadowCaster2D.enabled = false;

            if (isMobile)
            {
                col.enabled = false;

                if (transform.childCount > 0)
                {
                    Transform child = transform.GetChild(0);

                    if (child != null)
                        child.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            spriteRenderer.sprite = doorBaseSO.doorImage;
            if (shadowCaster2D != null) shadowCaster2D.enabled = true;

            if (isMobile)
            {
                col.enabled = true;

                if (transform.childCount > 0)
                {
                    Transform child = transform.GetChild(0);

                    if (child != null)
                        child.gameObject.SetActive(true);
                }
            }
        }

        yield return null;

        InteractableBase[] interactables = GameObject.FindObjectsByType<InteractableBase>(FindObjectsSortMode.None);
        foreach (var interactable in interactables)
        {
            foreach (int key in interactable.GetKey())
            {
                if (doorKey == key && !totalKeysActivated.Contains(interactable))
                {
                    totalKeysActivated.Add(interactable);
                }
            }
        }

        DoorBase[] allDoors = GameObject.FindObjectsByType<DoorBase>(FindObjectsSortMode.None);

        foreach (DoorBase door in allDoors)
        {
            if (door.doorKey == doorKeyTP)
            {
                teleportDestination = door.gameObject;
                doorTP = door;
                break;
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
            if (doorKey == key && totalKeysActivated.Contains(interactable))
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

        if (colliders.Length > 0)
        {
            GameObject kid = GameObject.FindGameObjectWithTag("Kid");

            if (kid.GetComponent<KidMachine>().GetCurrentState() == KidMachine.States.Hand)
                areTwoPlayersOnDoor = true;
            else
                areTwoPlayersOnDoor = false;
        }
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

    private void OpenDoor()
    {
        if (isMobile)
        {
            col.enabled = false;

            if (transform.childCount > 0)
            {
                Transform child = transform.GetChild(0);

                if (child != null)
                    child.gameObject.SetActive(false);
            }
        }

        if (keepActivated)
            stopProcessing = true;

        spriteRenderer.sprite = doorBaseSO.openDoorImage;
        thisActivated = true;

        if (shadowCaster2D != null) shadowCaster2D.enabled = false;
    }

    private void CloseDoor()
    {
        if (isMobile)
        {
            col.enabled = true;

            if (transform.childCount > 0)
            {
                Transform child = transform.GetChild(0);

                if (child != null)
                    child.gameObject.SetActive(true);
            }
        }

        spriteRenderer.sprite = doorBaseSO.doorImage;
        thisActivated = false;

        if (shadowCaster2D != null) shadowCaster2D.enabled = true;
    }

    public void Interacted()
    {
        if (areTwoPlayersOnDoor && thisActivated)
        {
            if (doorBaseSO != null && !isMobile && !doorBaseSO.isZoneDoor)
            {
                // RECARGAR HABILIDAD DE SALA (LIMITAR 1 VEZ POR PUERTA) --- DE MOMENTO NO HACE FALTA
                OnStartCamTransition?.Invoke(doorKey, doorTP.gameObject.transform);
            }
            else if (doorBaseSO != null && !isMobile && doorBaseSO.isZoneDoor && !doorBaseSO.isFinalDoor)
            {
                // RECARGAR HABILIDADES ENTERAS Y LLAMAR M�QUINA EXPRENDEDORA (YA NO PUEDE VOLVER ATR�S)
                GameManager.Instance.StopCounting();
                //OnZoneEnded?.Invoke(); --> Maquina
                GameManager.currentZone = GameManager.currentZone + 1;
                SceneChanger.instance.NextZone();
                GameManager.Instance.RestartTime();//BORRAR
            }
            else
            {
                GameManager.Instance.StopCounting();
                //LLEVAR AL FINAL DEL JUEGO
            }
        }
    }

    private void TeleportPlayers(int keyIdentificator)
    {
        if (teleportDestination != null && keyIdentificator == doorKey)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            GameObject kid = GameObject.FindGameObjectWithTag("Kid");

            Vector3 tpOffset = CalculateDynamicOffset(player.transform) * 2f;
            Vector3 destination = teleportDestination.transform.position + tpOffset;

            player.transform.position = destination;
            kid.transform.position = destination;

            foreach (var key in player.GetComponent<PlayerInventory>().GetKey())
            {
                if (key != null)
                    key.transform.position = destination;
            }

            foreach (var key in kid.GetComponent<PlayerInventory>().GetKey())
            {
                if (key != null)
                    key.transform.position = destination;
            }
        }
    }

    private Vector3 CalculateDynamicOffset(Transform player)
    {
        Vector3 toPlayer = player.position - transform.position;
        Vector3 axis = (detectionAxis == TPAxis.UpDown)
            ? transform.up
            : transform.right;

        float side = Vector3.Dot(toPlayer, axis);

        return side >= 0 ? -axis : axis;
    }

    public string GetTotalKeysActivated()
    {
        int count = 0;
        string text = "";

        foreach (InteractableBase interactable in totalKeysActivated)
        {
            if (interactable != null && interactable.GetIsActivated())
                count++;
        }

        text += " | Puerta: ";

        if (thisActivated)
            text += "Abierta | ";
        else
            text += "Cerrada | ";

        text += "Tipo: ";

        if (hasMultipleOptions && isIndependent)
            text += "M�ltiple Fija | ";
        else if (hasMultipleOptions && isIndependent)
            text += "M�ltiple Alterable | ";
        else
            text += "Dependiente | ";

        text += "Elementos activos: ";

        return text + "(" + count.ToString() + "/" + Mathf.Max(1, totalKeysActivated.Count) + ")";
    }


    public int GetDoorKey()
    {
        return doorKey;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void Interacted(GameObject other, GameObject thisObject) { }
}