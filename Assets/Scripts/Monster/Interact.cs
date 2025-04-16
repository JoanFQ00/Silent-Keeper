using System;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] float detectionRadius;
    public LayerMask detectionLayer;

    public static Action<GameObject, bool> ShowInteraction;
    private static bool kidDetected = false;
    private static bool playerDetected = false;
    private static Collider2D kidHit, MonsterHit;

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
                ShowInteraction?.Invoke(kidHit.gameObject, true);
            else
                ShowInteraction?.Invoke(MonsterHit.gameObject, true);
        }
        else
        {
            if (!kidDetected && !playerDetected)
            {
                ShowInteraction?.Invoke(null, false);
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

    private void OnInteractionExecutedMonster()
    {
        //CUANDO TENGAMOS LAS 2 TECLAS DIFERENTES
        //if (gameObject.tag == "Player")
        //    Interacting(this.gameObject);

        Interacting(this.gameObject);
    }

    private void OnInteractionExecutedKid()
    {
        if (gameObject.tag == "Kid")
            Interacting(this.gameObject);
    }

    public float GetDetectionRadius() { return detectionRadius; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}