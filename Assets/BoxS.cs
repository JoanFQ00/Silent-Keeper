using UnityEngine;

public class BoxS : MonoBehaviour, IInteractableWithButton
{
    public float forceAmount = 5f;
    private Rigidbody2D rb;
    private InteractableBase interactable;
    private SpriteRenderer spriteRenderer;
    private GameObject player;

    void Start()
    {
        interactable = GetComponent<InteractableBase>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.collider.gameObject.layer == LayerMask.GetMask())
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Interacted()
    {
        Vector2 diff = transform.position - player.transform.position;
        Vector2 direction;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            direction = new Vector2(Mathf.Sign(diff.x), 0f);
        }
        else
        {
            direction = new Vector2(0f, Mathf.Sign(diff.y));
        }

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.AddForce(direction * forceAmount, ForceMode2D.Impulse);
    }

    public void Interacted(GameObject other, GameObject thisObject) { }
}

