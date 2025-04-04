using UnityEngine;

public class PatrolBehaviour : MonoBehaviour
{
    public float Speed = 3f;
    public float WaitTime = 2f;
    public Transform[] PatrolPoints;

    private int _currentPoint;
    private float _waitCounter;
    private bool _waiting;
    private Transform _player;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _currentPoint = 0;
        _waiting = false;
        _waitCounter = 0;
    }

    void Update()
    {
        if (_waiting)
        {
            _waitCounter -= Time.deltaTime;
            if (_waitCounter <= 0)
            {
                _waiting = false;
                _currentPoint = (_currentPoint + 1) % PatrolPoints.Length;
            }
            return;
        }

        Transform targetPoint = PatrolPoints[_currentPoint];
        Vector2 direction = (targetPoint.position - transform.position).normalized;
        _rb.linearVelocity = direction * Speed;

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            _waiting = true;
            _waitCounter = WaitTime;
            _rb.linearVelocity = Vector2.zero;
        }

        RotateTowards(transform, direction);
    }

    private void RotateTowards(Transform transform, Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}