using UnityEngine;

public class BoxS : MonoBehaviour, IInteractableWithButton
{
    public float moveSpeed = 2f;
    private Vector2 moveDirection = Vector2.zero;
    private bool isMoving = false;

    private GameObject player;
    private Rigidbody2D rb;

    [SerializeField] private float stopTime = 0.2f; // Tiempo que se mueve la caja antes de detenerse
    private float moveTimer;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isMoving)
        {
            moveTimer -= Time.deltaTime;
            rb.linearVelocity = moveDirection * moveSpeed;

            if (moveTimer <= 0f)
            {
                rb.linearVelocity = Vector2.zero;
                isMoving = false;
            }
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
        moveTimer = stopTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si choca antes de terminar el tiempo, detenerse
        if (isMoving)
        {
            rb.linearVelocity = Vector2.zero;
            isMoving = false;
        }
    }

    public void Interacted(GameObject other, GameObject thisObject) { }
}
