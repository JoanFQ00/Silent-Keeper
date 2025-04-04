using System.Collections;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] private float patrolInterval = 2f;
    [SerializeField] private Vector2 patrolAreaMin;
    [SerializeField] private Vector2 patrolAreaMax;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float idleTime = 1f;

    private Vector2 nextPatrolPoint;
    private float patrolTimer;
    private Rigidbody2D rb;
    private bool isIdle = false;
    private float idleTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetNextPatrolPoint();
        patrolTimer = patrolInterval;
    }

    void Update()
    {
        if (isIdle)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0)
            {
                isIdle = false;
                SetNextPatrolPoint();
            }
            return;
        }

        patrolTimer -= Time.deltaTime;
        if (patrolTimer <= 0)
        {
            SetNextPatrolPoint();
            patrolTimer = patrolInterval;
        }
    }

    void FixedUpdate()
    {
        if (!isIdle)
        {
            Patrol();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Patrol()
    {
        Vector2 direction = (nextPatrolPoint - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * patrolSpeed;

        if (Vector2.Distance(transform.position, nextPatrolPoint) < 0.5f)
        {
            isIdle = true;
            idleTimer = idleTime;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void SetNextPatrolPoint()
    {
        nextPatrolPoint = new Vector2(
            Random.Range(patrolAreaMin.x, patrolAreaMax.x),
            Random.Range(patrolAreaMin.y, patrolAreaMax.y)
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Mapa"))
        {
            SetNextPatrolPoint();
        }
    }
}