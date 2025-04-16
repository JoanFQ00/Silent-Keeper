using UnityEngine;

public class PatrolBehaviour : MonoBehaviour
{
    public float Speed = 3f;
    public float WaitTime = 2f;
    public Transform[] PatrolPoints;

    private int _currentPoint;
    private float _waitCounter;
    private bool _waiting;

    private Rigidbody2D _rb;
    private Transform _player;
    private Transform targetPoint;
    private Transform nextPoint;

    private Vector2 direction;
    private Quaternion _startRotation;
    private Quaternion _targetRotation;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _currentPoint = 0;
        targetPoint = PatrolPoints[_currentPoint + 1];

        if (_currentPoint + 1 >= PatrolPoints.Length)
            nextPoint = PatrolPoints[0];
        else
            nextPoint = PatrolPoints[_currentPoint + 1]; 
        
        direction = (targetPoint.position - transform.position).normalized;

        _waiting = false;
        _waitCounter = 0;
    }

    void Update()
    {
        if (gameObject.GetComponent<EnemyDetector>().GetEnemyState() == EnemyDetector.EnemyState.NORMAL)
        {
            if (_waiting)
            {
                _waitCounter -= Time.deltaTime;

                float t = 1f - (_waitCounter / WaitTime);
                transform.rotation = Quaternion.Lerp(_startRotation, _targetRotation, t);

                if (_waitCounter <= 0.0f)
                {
                    _waiting = false;
                    _currentPoint = (_currentPoint + 1) % PatrolPoints.Length;

                    targetPoint = PatrolPoints[_currentPoint];
                    direction = (targetPoint.position - transform.position).normalized;

                    if (_currentPoint + 1 >= PatrolPoints.Length)
                        nextPoint = PatrolPoints[0];
                    else
                        nextPoint = PatrolPoints[_currentPoint + 1];
                }
                return;
            }

            if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
            {
                _waiting = true;
                _waitCounter = WaitTime;

                RotateEnemy();
            }
            else
            {
                _rb.linearVelocity = direction * Speed;
            }
        }
        else
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0f;
        }
    }

    private void RotateEnemy()
    {
        _rb.linearVelocity = Vector2.zero;
        _startRotation = transform.rotation;

        Vector3 newDirection = (nextPoint.position - transform.position).normalized;
        float signedAngle = Vector3.SignedAngle(direction, newDirection, Vector3.forward);
        _targetRotation = _startRotation * Quaternion.Euler(0, 0, signedAngle);
    }
}