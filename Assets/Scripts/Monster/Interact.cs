using System;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] float detectionRadius;
    public LayerMask detectionLayer;

    private static bool kidDetected = false;
    private static bool playerDetected = false;
    private static Collider2D kidHit, MonsterHit;
    private float scrollCooldown = 0.3f; //AQUÍ POSAR EL TEMPS QUE TRIGA EN REALITZAR L'ANIMACIÓ D'ENTRAR A LA TAQUILLA
    private float lastScrollTime = 0f;

    public static Action<GameObject> ShowInteractionMonster, ShowInteractionKid;

    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, detectionLayer);

        if (gameObject.CompareTag("Kid"))
        {
            kidHit = hit;
            kidDetected = hit != null;
        }

        if (gameObject.CompareTag("Player"))
        {
            MonsterHit = hit;
            playerDetected = hit != null;
        }

        if (hit != null)
        {
            if (gameObject.tag == "Kid")
            {
                if (gameObject.GetComponent<KidMachine>().GetCurrentState() != KidMachine.States.HiddenInLocker)
                    ShowInteractionKid?.Invoke(kidHit.gameObject);
            }
            else
            {
                if (gameObject.GetComponent<MonsterMove>().GetMonsterState() != KidMachine.States.HiddenInLocker)
                    ShowInteractionMonster?.Invoke(MonsterHit.gameObject);
            }
        }
        else
        {
            if (!playerDetected)
                ShowInteractionMonster?.Invoke(null);
            
            if (!kidDetected)
                ShowInteractionKid?.Invoke(null);
        }

        if (Input.GetKeyDown(KeyCode.F) && gameObject.tag == "Player")
        {
            if (!GameManager.Instance.isPaused)
            {
                ShowInteractionMonster?.Invoke(null);
                Interacting(gameObject);
            }
        }

        if (Input.mouseScrollDelta.y < 0 && gameObject.tag == "Kid")
        {
            if (!GameManager.Instance.isPaused && Time.time - lastScrollTime > scrollCooldown)
            {
                ShowInteractionKid?.Invoke(null);

                lastScrollTime = Time.time;
                Interacting(gameObject);
            }
        }
    }

    public void Interacting(GameObject thisObject)
    {
        if (this.gameObject == thisObject)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, detectionLayer);

            if (hit != null)
            {
                if (hit.GetComponent<Lever>() != null)
                {
                    var element = hit.GetComponent<Lever>();
                    element.Interacted();
                }
                else if (hit.GetComponent<DoorBase>() != null)
                {
                    var element = hit.GetComponent<DoorBase>();
                    element.Interacted();
                }
                else if (hit.GetComponent<Locker>() != null)
                {
                    if (this.gameObject == hit.GetComponent<Locker>().GetObjectContained() || hit.GetComponent<Locker>().GetObjectContained() == null)
                    {
                        var element = hit.GetComponent<Locker>();
                        element.Interacted(this.gameObject, hit.gameObject);
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

    public float GetDetectionRadius() { return detectionRadius; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}