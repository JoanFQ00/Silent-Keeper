using UnityEngine;

public class BoxBehaviour : MonoBehaviour, IInteractableWithButton
{
    public float moveSpeed = 2f;
    private Vector2 moveDirection;
    private GameObject player;
    private bool isMoving = false;
    [SerializeField] private float pushBackDistance = 0.2f;

    void Start()
    {
    }

    void Update()
    {
    }
    public void Interacted()
    {
        isMoving = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isMoving = false;
    }

    public void Interacted(GameObject other, GameObject thisObject) { }
}