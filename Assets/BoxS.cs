using UnityEngine;

public class BoxS : MonoBehaviour, IInteractableWithButton
{
    public float moveSpeed = 2f;
    private Vector2 moveDirection = Vector2.zero;
    private bool isMoving = false;

    private InteractableBase interactable;
    private SpriteRenderer spriteRenderer;
    private GameObject player;

    private Rigidbody2D rb;

    [SerializeField] private float pushBackDistance = 0.2f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    public void Interacted()
    {
        Vector2 diff = transform.position - player.transform.position;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            moveDirection = new Vector2(Mathf.Sign(diff.x), 0f);
        }
        else
        {
            moveDirection = new Vector2(0f, Mathf.Sign(diff.y));
        }

        isMoving = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isMoving = false;

        PushBackFromCollision();
    }

    private void PushBackFromCollision()
    {
        if (moveDirection == Vector2.zero) return;

        Vector3 pushDir = -((Vector3)moveDirection).normalized;
        transform.position += pushDir * pushBackDistance;
    }

    public void Interacted(GameObject other, GameObject thisObject) { }
}