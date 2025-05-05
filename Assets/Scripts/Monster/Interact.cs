using System;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] float interactRadius, detectionRadius;
    public LayerMask detectionLayer;

    private float scrollCooldown = 0.3f;
    private float lastScrollTime = 0f;

    //BORRAR
    //public float GetRadius(bool getInteractRadius)
    //{
    //    if (getInteractRadius)
    //        return interactRadius;
    //    else
    //        return detectionRadius;
    //}

    public float GetDetectionRadius()
    {
        return detectionRadius;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && gameObject.tag == "Kid")
        {
            if (!GameManager.Instance.isPaused)
                Interacting();
        }

        if (Input.GetKeyDown(KeyCode.F) && gameObject.tag == "Player")
        {
            if (!GameManager.Instance.isPaused)
                Interacting();
        }

        //if (Input.GetKeyDown(KeyCode.F) && gameObject.tag == "Kid")
        //{
        //    Interacting();
        //    // if (!GameManager.Instance.isPaused && Time.time - lastScrollTime > scrollCooldown)
        //    // {
        //    //     lastScrollTime = Time.time;
        //    // }
        //}

        InteractableDetection();
    }

    private void InteractableDetection()
    {
        Collider2D detectableHit = Physics2D.OverlapCircle(transform.position, detectionRadius, detectionLayer);
        Collider2D interactableHit = Physics2D.OverlapCircle(transform.position, interactRadius, detectionLayer);

        if (detectableHit != null)
        {
            if (interactableHit != null)
                interactableHit.GetComponent<InteractTxt>().Checker(true, true, gameObject.tag);
            else
                detectableHit.GetComponent<InteractTxt>().Checker(true, false, gameObject.tag);
        }
    }

    private void Interacting()
    {
        Collider2D interactableObj = Physics2D.OverlapCircle(transform.position, interactRadius, detectionLayer);

        if (interactableObj != null)
        {
            if (interactableObj.GetComponent<Lever>() != null)
            {
                var element = interactableObj.GetComponent<Lever>();
                element.Interacted();
            }
            else if (interactableObj.GetComponent<DoorBase>() != null)
            {
                var element = interactableObj.GetComponent<DoorBase>();
                element.Interacted();
            }
            else if (interactableObj.GetComponent<Locker>() != null)
            {
                if (gameObject == interactableObj.GetComponent<Locker>().GetObjectContained() || interactableObj.GetComponent<Locker>().GetObjectContained() == null)
                {
                    if (gameObject == interactableObj.GetComponent<Locker>().GetObjectContained())
                    {
                        var element = interactableObj.GetComponent<Locker>();
                        element.Interacted(gameObject, interactableObj.gameObject);
                    }
                    else
                    {
                        if (interactableObj.GetComponent<Locker>().GetObjectContained() == null && gameObject.tag == "Kid")
                        {
                            if (gameObject.GetComponent<KidMachine>().GetCurrentState() != KidMachine.States.HiddenInLocker)
                            {
                                var element = interactableObj.GetComponent<Locker>();
                                element.Interacted(gameObject, interactableObj.gameObject);
                            }
                        }
                        else if (interactableObj.GetComponent<Locker>().GetObjectContained() == null && gameObject.tag == "Player")
                        {
                            if (gameObject.GetComponent<MonsterMove>().GetMonsterState() != KidMachine.States.HiddenInLocker)
                            {
                                var element = interactableObj.GetComponent<Locker>();
                                element.Interacted(gameObject, interactableObj.gameObject);
                            }
                        }
                    }
                }
            }
        }
    }

    //INPUT SYSTEM
    //private void OnInteractionExecutedMonster()
    //{
    //    //CUANDO TENGAMOS LAS 2 TECLAS DIFERENTES
    //    if (!GameManager.Instance.isPaused)
    //    {
    //        if (gameObject.tag == "Player")
    //            Interacting(this.gameObject);

    //        Interacting(gameObject);
    //    }
    //}

    //private void OnInteractionExecutedKid()
    //{
    //    if (!GameManager.Instance.isPaused)
    //    {
    //        if (gameObject.tag == "Kid")
    //            Interacting(this.gameObject);
    //    }
    //}

    //public float GetInteractRadius() { return interacRadius; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}