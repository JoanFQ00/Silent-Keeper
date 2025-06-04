using UnityEngine;

public class KeyBase : MonoBehaviour
{
    [SerializeField] float distToTarget = 1f;
    [SerializeField] float followSpeed = 5f;

    private Transform followTarget;
    private InteractableBase interactable;
    private SpriteRenderer sr;
    private CircleCollider2D circleCollider;
    //private bool hasBeenUsed;

    public enum KeyType {blue, orange, ci,rc,us }
    public KeyType keyType;

    //private void OnEnable()
    //{
    //    PlayerInventory.KeyHasBeenUsed += HandleKeyUsed;
    //}

    //private void OnDisable()
    //{
    //    PlayerInventory.KeyHasBeenUsed -= HandleKeyUsed;
    //}

    private void Start()
    {
        //hasBeenUsed = false;
        interactable = GetComponent<InteractableBase>();
        sr = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    //private void Update()
    //{
    //    if (!interactable.GetIsActivated() || followTarget == null) return;

    //    float distance = Vector2.Distance(transform.position, followTarget.position);
    //    if (distance > distToTarget)
    //    {
    //        transform.position = Vector2.MoveTowards(transform.position, followTarget.position, followSpeed * Time.deltaTime);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!interactable.GetIsActivated() && collision.gameObject.layer == LayerMask.NameToLayer("Players"))
        {
            interactable.SetKeyActivated(true);
            collision.GetComponent<PlayerInventory>().OnKeyObtained(this);
            sr.enabled = false;
            circleCollider.enabled = false;
        }
    }

    //public void SetFollowTarget(Transform target)
    //{
    //    followTarget = target;
    //}

    //public void KeyHasBeenUsed()
    //{
    //    hasBeenUsed = true;
    //}

    //private void HandleKeyUsed()
    //{
    //    if (hasBeenUsed)
    //    {
    //        gameObject.SetActive(false);
    //    }
    //}
}