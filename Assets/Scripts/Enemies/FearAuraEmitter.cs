using UnityEngine;

public class FearAuraEmitter : MonoBehaviour
{
    [Range(1f, 50f)]
    public float detectionRadius = 0f;

    public Color dangerColor = Color.black; 

    void OnDrawGizmosSelected()
    {
        Gizmos.color = dangerColor;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}