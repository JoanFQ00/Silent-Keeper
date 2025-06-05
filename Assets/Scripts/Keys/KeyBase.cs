using UnityEngine;

public class KeyBase : MonoBehaviour
{
    private Transform followTarget;
    private InteractableBase interactable;
    private SpriteRenderer sr;
    private CircleCollider2D circleCollider;
    public enum KeyType { blue, orange, ci, rc, us }
    public KeyType keyType;

    private void Start()
    {
        interactable = GetComponent<InteractableBase>();
        sr = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

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
}