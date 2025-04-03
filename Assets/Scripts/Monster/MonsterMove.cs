using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterMove : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveDir;
    [SerializeField] float speed = 2f;
    [SerializeField] float deceleration = 5f; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (moveDir.magnitude > 0.1f) 
        {
            rb.linearVelocity = moveDir * speed;
        }
        else 
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.deltaTime * deceleration);
        }
    }

    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}