using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterMove : MonoBehaviour
{
    [SerializeField] float defaultSpeed = 2f;
    [SerializeField] float deceleration = 5f;
    private float currentSpeed;
    private Rigidbody2D rb;
    private Vector2 moveDir;
    private KidMachine.States monsterState;

    public static Action<string> OnChangeTxt;

    void Start()
    {
        monsterState = KidMachine.States.Stoped;
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = defaultSpeed;
    }

    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            if (moveDir.magnitude > 0.1f)
            {
                rb.linearVelocity = moveDir * currentSpeed;
            }
            else
            {
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.deltaTime * deceleration);
            }
        }
        else 
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void ChangeState(KidMachine.States state)
    {
        monsterState = state;
    }

    public KidMachine.States GetMonsterState()
    {
        return monsterState;
    }

    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }

    private void OnEscapeButt() 
    {
        GameManager.Instance.Pause();
    }

    public void SetSpeed(float _speed)
    {
        currentSpeed = _speed;
    }

    public float GetSpeed()
    {
        return currentSpeed;
    }

    //Al cambiar de zona
    public void SetDefaultSpeed() 
    {
        currentSpeed = defaultSpeed;
    }
}