using UnityEngine;

public class BoxS : MonoBehaviour, IInteractableWithButton
{
    public float moveSpeed = 2f; // Velocidad constante MRU
    private Vector2 moveDirection = Vector2.zero;
    private bool isMoving = false;

    private InteractableBase interactable;
    private SpriteRenderer spriteRenderer;
    private GameObject player;

    void Start()
    {
        //interactable = GetComponent<InteractableBase>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
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
    }

    public void Interacted(GameObject other, GameObject thisObject) { }
}
