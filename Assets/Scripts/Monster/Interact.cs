using System;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] float detectionRadius;
    public LayerMask detectionLayer;
    //private Lever lever;

    public static Action<bool> ShowInteraction;

    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, detectionLayer);

        if (hit != null) 
        {
            ShowInteraction?.Invoke(true);
        }
        else 
        {
            ShowInteraction?.Invoke(false);
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
                    var element = hit.GetComponent<Locker>();
                    element.Interacted(this.gameObject, hit.gameObject);
                }
            }
        }
    }

    private void OnInteractionExecuted()
    {
        //Interact only if controlling Monster
        if (GetComponent<MonsterMove>().GetCurrentMode() == MonsterMove.Mode.KidCon)
        {
            Interacting(GameObject.FindGameObjectWithTag("Kid"));
        }
        else
        {
            Interacting(GameObject.FindGameObjectWithTag("Player"));
        }
    }

    public float GetDetectionRadius() { return detectionRadius; }

    private void OnDrawGizmos()
    {
        // Establece el color del Gizmo (puedes cambiarlo si prefieres otro color)
        Gizmos.color = Color.red;

        // Dibuja un círculo en la posición del objeto con el radio de detección
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
