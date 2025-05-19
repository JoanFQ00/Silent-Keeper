using UnityEngine;

public class TorchBase : MonoBehaviour, IInteractableWithButton
{
    [HideInInspector]
    public static bool pickUp = false;

    [Header("Settings")]
    public float followDuration = 5f;
    public float followSpeed = 3f;
    public float followDistance = 1.5f;
    public float detectionRadius = 1f;
    public LayerMask interactableLayer;

    [Header("References")]
    public GameObject torch;

    private float followTimer = 0f;
    private Transform playerTarget = null;

    private void Start()
    {
        pickUp = false;
    }

    private void Update()
    {
        if (pickUp)
        {
            followTimer += Time.deltaTime;

            if (playerTarget != null)
            {
                float distance = Vector2.Distance(torch.transform.position, playerTarget.position);

                if (distance > followDistance)
                {
                    Vector2 dir = (playerTarget.position - torch.transform.position).normalized;
                    torch.transform.position += (Vector3)(dir * followSpeed * Time.deltaTime);
                }
            }

            if (followTimer >= followDuration)
            {
                torch.GetComponent<InteractableBase>().SetKeyActivated(false);
                pickUp = false;
                followTimer = 0f;
                ReturnToSpawn();
            }

            CheckInteractableCollisions();
        }
    }

    private void CheckInteractableCollisions()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(torch.transform.position, detectionRadius, interactableLayer);

        foreach (Collider2D hit in hits)
            if (hit.TryGetComponent<DoorBase>(out DoorBase door))
                foreach (int key in torch.GetComponent<InteractableBase>().GetKey())
                    if (door.GetDoorKey() == key)
                    {
                        torch.GetComponent<InteractableBase>().SetKeyActivated(true);
                        torch.SetActive(false);
                        break;
                    }
    }

    private void ReturnToSpawn()
    {
        torch.transform.position = transform.position;
    }

    public void Interacted() { }

    public void Interacted(GameObject other, GameObject thisObject)
    {
        if (!pickUp && torch.activeSelf)
        {
            playerTarget = other.transform;
            torch.transform.position = playerTarget.position;
            pickUp = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (torch != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(torch.transform.position, detectionRadius);
        }
    }
}