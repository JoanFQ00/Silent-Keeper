using UnityEngine;

public class KeyBase : MonoBehaviour
{
    [SerializeField] float distToTarget = 1f;
    [SerializeField] float followSpeed = 5f;

    private Transform followTarget;
    private InteractableBase interactable;
    private bool hasBeenUsed;

    private void OnEnable()
    {
        PlayerInventory.KeyHasBeenUsed += DestroyKey;
    }

    private void OnDisable()
    {
        PlayerInventory.KeyHasBeenUsed -= DestroyKey;
    }

    private void Start()
    {
        hasBeenUsed = false;
        interactable = GetComponent<InteractableBase>();
    }

    private void Update()
    {
        if (!interactable.GetIsActivated() || followTarget == null) return;

        float distance = Vector2.Distance(transform.position, followTarget.position);
        if (distance > distToTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, followTarget.position, followSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!interactable.GetIsActivated() && collision.gameObject.layer == LayerMask.NameToLayer("Players"))
        {
            interactable.SetKeyActivated(true);
            collision.GetComponent<PlayerInventory>().OnKeyObtained(this);
        }
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }

    public void KeyHasBeenUsed()
    {
        hasBeenUsed = true;
    }

    private void DestroyKey()
    {
        if (hasBeenUsed)
            Destroy(gameObject);
    }
}