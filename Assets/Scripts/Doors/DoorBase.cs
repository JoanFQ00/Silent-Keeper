using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [Header("Particles")]
    [SerializeField] GameObject fireParticle1, fireParticle2;

    [SerializeField] bool hasToBeAnimated, penultimateDoor, circusDoor = false;

    private List<InteractableBase> totalKeysActivated = new List<InteractableBase>();
    private Dictionary<InteractableBase, bool> copyKeyStates = new Dictionary<InteractableBase, bool>();
    private SpriteRenderer spriteRenderer;
    private ShadowCaster2D shadowCaster2D;
    private Collider2D col;
    private GameObject teleportDestination;
    private bool areTwoPlayersOnDoor, stopProcessing = true, penultimateDoorOpened = false;
    private float detectionRadius;
    private DoorBase doorTP;
    private Animator animator;

    public static Action OnZoneEnded;
    public static Action<KeyBase> RefreshPlayerInventory;
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

    private Vector2 colliderCenter;

    private IEnumerator Start()
    {
        stopProcessing = true;

        shadowCaster2D = GetComponentInChildren<ShadowCaster2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            colliderCenter = (Vector2)transform.position + collider.offset;

            float scaleFactor = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
            detectionRadius = collider.radius * scaleFactor;
        }
        else
        {
            colliderCenter = (Vector2)transform.position;
            detectionRadius = 1.0f;
        }

        if (doorBaseSO != null)
            spriteRenderer.sprite = doorBaseSO.doorImage;

        if (thisActivated)
        {
            spriteRenderer.sprite = doorBaseSO.openDoorImage;

            if (isMobile)
                OpenDoor();
            else
                OpenFirstDoor();
        }
        else
        {
            spriteRenderer.sprite = doorBaseSO.doorImage;

            if (isMobile)
                CloseDoor();
            else
                CloseFirstDoor();
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

                if (penultimateDoor)
                    doorTP.SetOtherPenultimateDoor();
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
                RefreshPlayerInventory?.Invoke(interactable.gameObject.GetComponent<KeyBase>());
                //interactable.gameObject.GetComponent<KeyBase>().KeyHasBeenUsed();
            }
        }
    }

    private void Update()
    {
        int playerLayerMask = 1 << LayerMask.NameToLayer("Players");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(colliderCenter, detectionRadius, playerLayerMask);

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

            if (canDoorBeOpened && !thisActivated && !hasToBeAnimated)
                OpenDoor();

            foreach (InteractableBase interactable in totalKeysActivated)
            {
                if (!interactable.GetIsActivated())
                    if (thisActivated && !hasToBeAnimated)
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

                if (canDoorBeOpened && !thisActivated && !hasToBeAnimated)
                    OpenDoor();

                foreach (InteractableBase interactable in totalKeysActivated)
                {
                    if (interactable.GetIsActivated())
                    {
                        canDoorBeClosed = false;
                        break;
                    }
                }

                if (canDoorBeClosed && thisActivated && !hasToBeAnimated)
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
                        if (thisActivated && !hasToBeAnimated)
                            CloseDoor();
                        else if (!hasToBeAnimated)
                            OpenDoor();
                    }

                    copyKeyStates.Clear();
                    foreach (InteractableBase interactable in totalKeysActivated)
                        copyKeyStates[interactable] = interactable.GetIsActivated();
                }
            }
        }
    }

    private void OpenFirstDoor()
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

        if (circusDoor)
            spriteRenderer.sprite = doorBaseSO.openDoorImage;

        thisActivated = true;
        OnDoorChanged(true);
    }

    private void CloseFirstDoor()
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

        if (circusDoor)
            spriteRenderer.sprite = doorBaseSO.doorImage;

        thisActivated = false;
        OnDoorChanged(false);
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

        if (circusDoor)
            spriteRenderer.sprite = doorBaseSO.openDoorImage;
        else if (animator != null)
            animator.SetTrigger("Open");

        thisActivated = true;

        OnDoorChanged(true);
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

        if (circusDoor)
            spriteRenderer.sprite = doorBaseSO.doorImage;
        else if (animator != null)
            animator.SetTrigger("Close");

        thisActivated = false;

        OnDoorChanged(false);
    }

    public void Interacted()
    {
        if (areTwoPlayersOnDoor && thisActivated && (!penultimateDoor || (penultimateDoor && !penultimateDoorOpened)))
        {
            if (doorBaseSO != null && !isMobile && !doorBaseSO.isZoneDoor)
            {
                // RECARGAR HABILIDAD DE SALA (LIMITAR 1 VEZ POR PUERTA) --- DE MOMENTO NO HACE FALTA
                if (penultimateDoor)
                {
                    doorTP.CloseDoor();
                    penultimateDoorOpened = true;
                }

                OnStartCamTransition?.Invoke(doorKey, doorTP.gameObject.transform);
            }
            else if (doorBaseSO != null && !isMobile && doorBaseSO.isZoneDoor && !doorBaseSO.isFinalDoor)
            {
                // RECARGAR HABILIDADES ENTERAS Y LLAMAR M�QUINA EXPRENDEDORA (YA NO PUEDE VOLVER ATR�S)
                GameManager.Instance.StopCounting();
                //OnZoneEnded?.Invoke(); --> Maquina
                GameManager.currentZone = GameManager.currentZone + 1;
                SceneChanger.instance.NextZone();
                //GameManager.Instance.RestartTime();//BORRAR
            }
            else
            {
                GameManager.Instance.StopCounting();
                SceneChanger.instance.LoadMenu();
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

            player.transform.position = destination + Vector3.left * 0.5f;
            kid.transform.position = destination + Vector3.right * 0.5f;

            //foreach (var key in player.GetComponent<PlayerInventory>().GetKey())
            //{
            //    if (key != null)
            //        key.transform.position = destination;
            //}

            //foreach (var key in kid.GetComponent<PlayerInventory>().GetKey())
            //{
            //    if (key != null)
            //        key.transform.position = destination;
            //}
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

    //public string GetTotalKeysActivated()
    //{
    //    int count = 0;
    //    string text = "";

    //    foreach (InteractableBase interactable in totalKeysActivated)
    //    {
    //        if (interactable != null && interactable.GetIsActivated())
    //            count++;
    //    }

    //    text += " | Puerta: ";

    //    if (thisActivated)
    //        text += "Abierta | ";
    //    else
    //        text += "Cerrada | ";

    //    text += "Tipo: ";

    //    if (hasMultipleOptions && isIndependent)
    //        text += "M�ltiple Fija | ";
    //    else if (hasMultipleOptions && isIndependent)
    //        text += "M�ltiple Alterable | ";
    //    else
    //        text += "Dependiente | ";

    //    text += "Elementos activos: ";

    //    return text + "(" + count.ToString() + "/" + Mathf.Max(1, totalKeysActivated.Count) + ")";
    //}

    public int GetDoorKey()
    {
        return doorKey;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(colliderCenter, detectionRadius);
    }

    public void OnDoorChangedAnim(bool active)
    {
        if (active)
            OpenDoor();
        else
            CloseDoor();
    }

    public void OnDoorChanged(bool active)
    {
        if (shadowCaster2D != null) shadowCaster2D.enabled = !active;

        foreach (var light in GetComponentsInChildren<Light2D>())
            light.enabled = active;

        if (fireParticle1 != null && fireParticle2 != null)
        {
            fireParticle1.SetActive(active);
            fireParticle2.SetActive(active);
        }
    }

    public void SetKeepActivated(bool activate)
    {
        keepActivated = activate;
    }

    public void SetOtherPenultimateDoor()
    {
        if (!penultimateDoor)
        {
            penultimateDoor = true;
            penultimateDoorOpened = true;
        }
    }

    public void Interacted(GameObject other, GameObject thisObject) { }
}