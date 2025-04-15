using System;
using Unity.VisualScripting;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] float detectionRadius;
    public LayerMask detectionLayer;

    public static Action<GameObject, bool> ShowInteraction;
    private static bool kidDetected = false;
    private static bool playerDetected = false;
    private static Collider2D kidHit, MonsterHit;
    //private static MonsterMove.Mode lastMode;

    //private void Start()
    //{
    //    lastMode = GameObject.FindGameObjectWithTag("Player").GetComponent<MonsterMove>().GetCurrentMode();
    //}

    //void Update()
    //{
    //    if (lastMode != GameObject.FindGameObjectWithTag("Player").GetComponent<MonsterMove>().GetCurrentMode())
    //    {
    //        ShowInteraction?.Invoke(null, false);
    //        lastMode = GameObject.FindGameObjectWithTag("Player").GetComponent<MonsterMove>().GetCurrentMode();
    //    }
    //    else
    //    {
    //        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, detectionLayer);

    //        if (gameObject.CompareTag("Kid"))
    //        {
    //            kidHit = hit;
    //            kidDetected = hit != null;
    //        }

    //        if (gameObject.CompareTag("Player"))
    //        {
    //            MonsterHit = hit;
    //            playerDetected = hit != null;
    //        }
       
    //        if (hit != null)
    //        {
    //            if (gameObject.CompareTag("Kid"))
    //            {
    //                if (GameObject.FindGameObjectWithTag("Player").GetComponent<MonsterMove>().GetCurrentMode() == MonsterMove.Mode.KidCon)
    //                    ShowInteraction?.Invoke(kidHit.gameObject, true); 
                    
    //            }
    //            else 
    //            {
    //                if (GameObject.FindGameObjectWithTag("Player").GetComponent<MonsterMove>().GetCurrentMode() == MonsterMove.Mode.MonsterCon && gameObject.tag == "Player")
    //                    ShowInteraction?.Invoke(MonsterHit.gameObject, true); 
    //            }
    //        }
    //        else
    //        {
    //            if (!kidDetected && !playerDetected)
    //            {
    //                ShowInteraction?.Invoke(null, false);
    //            }
    //        }
    //    }
    //}

    //public void Interacting(GameObject thisObject) 
    //{
    //    if (this.gameObject == thisObject)
    //    {
    //        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, detectionLayer);

    //        if (hit != null)
    //        {
    //            if (hit.GetComponent<Lever>() != null)
    //            {
    //                var element = hit.GetComponent<Lever>();
    //                element.Interacted();
    //            }
    //            else if (hit.GetComponent<Locker>() != null)
    //            {
    //                if (this.gameObject == hit.GetComponent<Locker>().GetObjectContained() || hit.GetComponent<Locker>().GetObjectContained() == null)
    //                {
    //                    var element = hit.GetComponent<Locker>();
    //                    element.Interacted(this.gameObject, hit.gameObject);
    //                }
    //            }
    //        }
    //    }
    //}

    //private void OnInteractionExecuted()
    //{
    //    MonsterMove monsterMove = gameObject.GetComponent<MonsterMove>();

    //    if (monsterMove != null)
    //    {
    //        if (GameObject.FindGameObjectWithTag("Player").GetComponent<MonsterMove>().GetCurrentMode() == MonsterMove.Mode.KidCon)
    //        {
    //            GameObject.FindGameObjectWithTag("Kid").GetComponent<Interact>().Interacting(GameObject.FindGameObjectWithTag("Kid"));
    //        }
    //        else
    //        {
    //            Interacting(GameObject.FindGameObjectWithTag("Player"));
    //        }
    //    }
    //}

    //public float GetDetectionRadius() { return detectionRadius; }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, detectionRadius);
    //}
}