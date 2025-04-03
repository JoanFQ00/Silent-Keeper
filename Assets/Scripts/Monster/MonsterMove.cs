using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterMove : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveDir;
    [SerializeField] float speed = 2f;
    [SerializeField] float deceleration = 5f;

    public enum Mode { MonsterCon, KidCon }
    public Mode currentMode;

    public static Action<string> OnChangeTxt; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMode = Mode.MonsterCon;
    }

    void Update()
    {
        if (moveDir.magnitude > 0.1f) 
        {
            rb.linearVelocity = moveDir * speed;
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
        OnChangeTxt?.Invoke("MonsterCon");
    }

    private void OnLeftMouse() 
    {
        //Interact only if controlling Monster
        if (currentMode == Mode.MonsterCon) 
        {
            Debug.Log("interacting");
        }
    }

    private void OnRightMouse() 
    {
        if (currentMode == Mode.MonsterCon)
        {
            currentMode = Mode.KidCon;
            OnChangeTxt?.Invoke("KidCon");
        }
    }
}
