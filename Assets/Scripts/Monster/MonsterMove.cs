using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterMove : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveDir;
    [SerializeField] float defaultSpeed = 2f;
    private float currentSpeed;
    [SerializeField] float deceleration = 5f;
    KidMachine kidMachine;
    public CameraFollow cameraFollow;
    public static Action<string> OnChangeTxt;
    private KidMachine.States monsterState;

    void Start()
    {
        monsterState = KidMachine.States.Stoped;
        rb = GetComponent<Rigidbody2D>();
        kidMachine = GameObject.FindGameObjectWithTag("Kid").GetComponent<KidMachine>();
        cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        currentSpeed = defaultSpeed;
    }

    void Update()
    {
        if (moveDir.magnitude > 0.1f) 
        {
            rb.linearVelocity = moveDir * currentSpeed;
        }
        else // When input out, inercy on movement
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.deltaTime * deceleration); 
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
        cameraFollow.ChangeTarget(GetComponentInParent<Transform>());
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
