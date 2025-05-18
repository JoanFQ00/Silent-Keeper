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
    private Animator playerAnimator;
    private Vector2 moveDir, lastNonZeroDir = Vector2.right;
    private KidMachine.States monsterState, monsterHiddenState;
    private Vector3 initialScale;

    public static Action<string> OnChangeTxt;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalColor = GetComponent<SpriteRenderer>().color;

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        playerAnimator = GetComponent<Animator>();

        initialScale = transform.localScale;
        currentSpeed = defaultSpeed;
        monsterState = KidMachine.States.Stoped;
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.isPaused)
        {
            StopMovement();

            if (playerAnimator != null)
            {
                playerAnimator.SetFloat("Horizontal", 0f);
                playerAnimator.SetFloat("Vertical", 0f);
                playerAnimator.SetFloat("Speed", 0f);
            }

            return;
        }

        if (moveDir.magnitude > 0.1f && !TransitionCamAnimation.Instance.playersInTransition)
        {
            rb.linearVelocity = moveDir.normalized * currentSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.fixedDeltaTime * deceleration);
        }
    }

    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
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

        if (Mathf.Abs(moveDir.x) > 0.01f)
        {
            if (moveDir.x < 0)
                transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
            else if (moveDir.x > 0)
                transform.localScale = initialScale;
        }

        if (moveDir.sqrMagnitude > 0.01f)
        {
            lastNonZeroDir = moveDir;
        }

        if (playerAnimator != null && !GameManager.Instance.isPaused)
        {
            playerAnimator.SetFloat("Horizontal", lastNonZeroDir.x);
            playerAnimator.SetFloat("Vertical", lastNonZeroDir.y);
            playerAnimator.SetBool("Speed", moveDir.sqrMagnitude > 0.01f);
        }
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