using System;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] float detectionRadius;
    public LayerMask detectionLayer;
    private Lever lever;

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

    public void Interacting() 
    {
         Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, detectionLayer);
         
         if (hit != null) 
         {
            lever = hit.GetComponent<Lever>();
            lever.Interacted();
         }
    }

     private void OnLeftMouse() 
    {
        //Interact only if controlling Monster
        if (GetComponent<MonsterMove>().GetCurrentMode() == MonsterMove.Mode.MonsterCon) 
        {
           Interacting();
        }
    }
}
