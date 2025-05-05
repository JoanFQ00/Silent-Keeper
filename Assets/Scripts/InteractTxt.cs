using System;
using TMPro;
using UnityEngine;

public class InteractTxt : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private string interacText;

    [SerializeField] float interactableArea = 2f;
    [SerializeField] GameObject keyMonsterUI, keyKidUI;
    [SerializeField] private SpriteRenderer iconSR;
    [SerializeField] private Sprite detectionIcon, interactIcon;

    private GameObject monster, kid;
    private LayerMask playersLayer;
    private DoorBase door;
    private string basicText, lastKeyText = "";

    public static Action<bool, string> OnShowMonsterTextUI, OnShowKidTextUI;

    private void OnEnable()
    {
        if (keyMonsterUI != null && keyKidUI != null)
        {
            InteractTxt.OnShowMonsterTextUI += ShowMonsterText;
            InteractTxt.OnShowKidTextUI += ShowKidText;
        }
    }

    private void OnDisable()
    {
        if (keyMonsterUI != null && keyKidUI != null)
        {
            InteractTxt.OnShowMonsterTextUI -= ShowMonsterText;
            InteractTxt.OnShowKidTextUI -= ShowKidText;
        }
    }

    private void Start()
    {
        monster = GameObject.FindGameObjectWithTag("Player");
        kid = GameObject.FindGameObjectWithTag("Kid");

        door = GetComponent<DoorBase>();

        basicText = interacText;

        playersLayer = LayerMask.GetMask("Players");

        if (keyMonsterUI != null && keyKidUI != null)
        {
            keyMonsterUI.SetActive(false);
            keyKidUI.SetActive(false);
        }

        Collider2D[] allColliders = GetComponents<Collider2D>();

        foreach (var col in allColliders)
        {
            if (col is CircleCollider2D circle && circle.isTrigger)
            {
                float maxScale = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y);
                circle.radius = interactableArea / maxScale;
            }
        }
    }

    private void Update()
    {
        string fullText = basicText;

        if (door != null)
        {
            string currentKeyText = door.GetTotalKeysActivated();
            if (lastKeyText != currentKeyText)
            {
                lastKeyText = currentKeyText;
            }

            fullText += lastKeyText;
            interacText = fullText;
        }

        //Checker(monster.GetComponent<Interact>().GetRadius(false), monster.GetComponent<Interact>().GetRadius(true), "Player");
        //Checker(kid.GetComponent<Interact>().GetRadius(false), kid.GetComponent<Interact>().GetRadius(true), "Kid");

        if ((monster.transform.position - transform.position).magnitude - monster.GetComponent<Interact>().GetDetectionRadius() > interactableArea
            && (kid.transform.position - transform.position).magnitude - kid.GetComponent<Interact>().GetDetectionRadius() > interactableArea)
            ChangeIcon(null);
    }

    //private void Checker(float detectionRadius, float interactionRadius, string tag)
    //{
    //    Collider2D detectionHit = Physics2D.OverlapCircle(transform.position, detectionRadius, playersLayer);
    //    Collider2D interactionHit = Physics2D.OverlapCircle(transform.position, interactionRadius, playersLayer);

    //    if (detectionHit != null)
    //    {
    //        if (interactionHit != null)
    //        {
    //            if (tag == monster.tag && monster.GetComponent<MonsterMove>().GetMonsterState() != KidMachine.States.HiddenInLocker)
    //            {
    //                VisualMonsterText(true);
    //                ChangeIcon(interactIcon);
    //            }
    //            else if (tag == kid.tag && kid.GetComponent<KidMachine>().GetCurrentState() != KidMachine.States.HiddenInLocker)
    //            {
    //                VisualKidText(true);
    //                ChangeIcon(interactIcon);
    //            }
    //        }
    //        else
    //        {
    //            if (tag == monster.tag)
    //                VisualMonsterText(false);
    //            else
    //                VisualKidText(false);

    //            ChangeIcon(detectionIcon);
    //        }
    //    }
    //    else
    //        ChangeIcon(null);
    //}

    public float GetInteractableArea()
    {
        return interactableArea;
    }

    public void Checker(bool isInDetectDistance, bool isInInteractDistance, string playerTag)
    {
        if (isInDetectDistance)
        {
            if (isInInteractDistance)
            {
                if (playerTag == monster.tag && monster.GetComponent<MonsterMove>().GetMonsterState() != KidMachine.States.HiddenInLocker)
                {
                    VisualMonsterText(true);
                    ChangeIcon(interactIcon);
                }
                else if (playerTag == kid.tag && kid.GetComponent<KidMachine>().GetCurrentState() != KidMachine.States.HiddenInLocker)
                {
                    VisualKidText(true);
                    ChangeIcon(interactIcon);
                }
            }
            else
            {
                if (playerTag == monster.tag)
                    VisualMonsterText(false);
                else
                    VisualKidText(false);

                ChangeIcon(detectionIcon);
            }
        }
    }

    private void ChangeIcon(Sprite newIcon)
    {
        if (iconSR != null)
        {
            if (newIcon == null)
                iconSR.enabled = false;
            else
            {
                if (iconSR.enabled == false)
                    iconSR.enabled = true;

                if (iconSR.sprite != newIcon)
                    iconSR.sprite = newIcon;
            }
        }
    }

    private void VisualMonsterText(bool activate)
    {
        if (keyMonsterUI == null && keyKidUI == null)
            OnShowMonsterTextUI?.Invoke(activate, interacText);
    }

    private void VisualKidText(bool activate)
    {
        if (keyMonsterUI == null && keyKidUI == null)
            OnShowKidTextUI?.Invoke(activate, interacText);
    }

    private void ShowMonsterText(bool activate, string textUI)
    {
        keyMonsterUI.SetActive(activate);
        keyMonsterUI.GetComponent<TextMeshProUGUI>().text = textUI;
    }

    private void ShowKidText(bool activate, string textUI)
    {
        keyKidUI.SetActive(activate);
        keyKidUI.GetComponent<TextMeshProUGUI>().text = textUI;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, interactableArea);
    }
}