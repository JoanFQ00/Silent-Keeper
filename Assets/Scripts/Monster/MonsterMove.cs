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
    public enum Mode { MonsterCon, KidCon }
    private Mode currentMode;
    KidMachine kidMachine;
    public CameraFollow cameraFollow;
    public static Action<string> OnChangeTxt; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMode = Mode.MonsterCon;
        OnChangeTxt?.Invoke(currentMode.ToString());
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

    private void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
        currentMode = Mode.MonsterCon;
        cameraFollow.ChangeTarget(GetComponentInParent<Transform>());
        OnChangeTxt?.Invoke(currentMode.ToString());
    }

    private void OnRightMouse() 
    {
        if (currentMode == Mode.MonsterCon)
        {
            currentMode = Mode.KidCon;
            cameraFollow.ChangeTarget(kidMachine.GetComponentInParent<Transform>());
            OnChangeTxt?.Invoke(currentMode.ToString());
        }
    }

    public Mode GetCurrentMode() 
    {
        return currentMode;
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

    public void ChangeMode(Mode mode) 
    {
        currentMode = mode;

        if (currentMode == Mode.KidCon)
        {
            cameraFollow.ChangeTarget(kidMachine.GetComponentInParent<Transform>());
            OnChangeTxt?.Invoke(currentMode.ToString());
        }
        else 
        {
            cameraFollow.ChangeTarget(GetComponent<Transform>());
            OnChangeTxt?.Invoke(currentMode.ToString());
        }
    }
}
