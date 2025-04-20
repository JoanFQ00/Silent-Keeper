using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterMove : MonoBehaviour
{
    [SerializeField] float defaultSpeed = 2f;
    [SerializeField] float deceleration = 5f;

    [HideInInspector]
    public int bushCounter = 0;
    public Color originalColor;

    private float currentSpeed;
    private Rigidbody2D rb;
    private Vector2 moveDir;
    private KidMachine.States monsterState, monsterHiddenState;

    public static Action<string> OnChangeTxt;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalColor = GetComponent<SpriteRenderer>().color;

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        currentSpeed = defaultSpeed;
        monsterState = KidMachine.States.Stoped;
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.isPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (moveDir.magnitude > 0.1f)
        {
            rb.linearVelocity = moveDir.normalized * currentSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.fixedDeltaTime * deceleration);
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

    public void ChangeHiddenState(KidMachine.States state)
    {
        monsterHiddenState = state;
    }

    public KidMachine.States GetHiddenMonsterState()
    {
        return monsterHiddenState;
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

    public void SetDefaultSpeed()
    {
        currentSpeed = defaultSpeed;
    }
}