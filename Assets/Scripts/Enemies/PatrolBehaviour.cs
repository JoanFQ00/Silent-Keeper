using UnityEngine;
using UnityEngine.Rendering.Universal;
using static EnemyDetector;

public class PatrolBehaviour : MonoBehaviour
{
    public float Speed = 3f;
    public float WaitTime = 2f;
    public float RotationTime = 1f;
    public Transform[] PatrolPoints;

    private int _currentPoint;
    private float _waitTimer;
    private float _rotationTimer;

    private bool waiting;
    private bool rotating;

    private Rigidbody2D _rb;
    private Light2D _light;
    private Transform targetPoint;

    private Vector2 direction;
    private Quaternion startRotation;
    private Quaternion targetRotation;

    private EnemyState patrolState;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _light = GetComponent<Light2D>();

        _currentPoint = 0;
        transform.position = PatrolPoints[_currentPoint].position;

        _currentPoint = (_currentPoint + 1) % PatrolPoints.Length;
        targetPoint = PatrolPoints[_currentPoint];
        direction = (targetPoint.position - transform.position).normalized;

        patrolState = EnemyState.NORMAL;
        waiting = false;
        rotating = false;
    }

    void Update()
    {
        if (gameObject.GetComponent<EnemyDetector>().GetEnemyState() != EnemyState.NORMAL || patrolState != EnemyState.NORMAL)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        if (waiting)
        {
            _rb.linearVelocity = Vector2.zero;
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0f)
            {
                waiting = false;
                StartRotation();
            }
            return;
        }

        if (rotating)
        {
            _rb.linearVelocity = Vector2.zero;
            _rotationTimer -= Time.deltaTime;

            float t = 1f - (_rotationTimer / RotationTime);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            if (_rotationTimer <= 0f)
            {
                rotating = false;
                if (_light != null) _light.enabled = true;

                _currentPoint = (_currentPoint + 1) % PatrolPoints.Length;
                targetPoint = PatrolPoints[_currentPoint];
                direction = (targetPoint.position - transform.position).normalized;
            }
            return;
        }

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            waiting = true;
            _waitTimer = WaitTime;
            return;
        }

        _rb.linearVelocity = direction * Speed;
    }

    void StartRotation()
    {
        rotating = true;
        _rotationTimer = RotationTime;
        startRotation = transform.rotation;

        Vector3 newDirection = (PatrolPoints[(_currentPoint + 1) % PatrolPoints.Length].position - transform.position).normalized;
        float signedAngle = Vector3.SignedAngle(direction, newDirection, Vector3.forward);
        targetRotation = startRotation * Quaternion.Euler(0, 0, signedAngle);

        if (_light != null) _light.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("StopWall"))
        {
            if (patrolState != EnemyState.STUNED)
            {
                gameObject.GetComponent<EnemyDetector>().SetEnemyState(EnemyState.STUNED);
                patrolState = EnemyState.STUNED;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("StopWall"))
        {
            if (patrolState != EnemyState.NORMAL)
            {
                gameObject.GetComponent<EnemyDetector>().SetEnemyState(EnemyState.NORMAL);
                patrolState = EnemyState.NORMAL;
            }
        }
    }
}