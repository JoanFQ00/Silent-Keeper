using UnityEngine;
using static StaticGuard;

public class PatrolBehaviour : MonoBehaviour
{
    public float Speed = 3f;
    public float WaitTime = 2f;
    public Transform[] PatrolPoints;

    public float RotationAngle = 180f;

    private int _currentPoint;
    private float _waitCounter;
    private bool _waiting;
    private Transform _player;
    private Rigidbody2D _rb;

    private Quaternion _startRotation;
    private Quaternion _targetRotation;

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
        //if (_waiting)
        //{
        //    _waitCounter -= Time.deltaTime;
        //    RotateTowards(transform, direction);

        //    if (_waitCounter <= 0)
        //    {
        //        _waiting = false;
        //        _currentPoint = (_currentPoint + 1) % PatrolPoints.Length;
        //    }
        //    return;
        //}

        if (gameObject.GetComponent<Detector>().GetEnemyState() == Detector.EnemyState.NORMAL)
        {
            Transform targetPoint = PatrolPoints[_currentPoint];
            Vector2 direction = (targetPoint.position - transform.position).normalized;

            if (_waiting)
            {
                _waitCounter -= Time.deltaTime;

                float t = 1f - (_waitCounter / WaitTime); // Normalizado de 0 a 1
                transform.rotation = Quaternion.Lerp(_startRotation, _targetRotation, t);

                if (_waitCounter <= 0)
                {
                    _waiting = false;
                    _currentPoint = (_currentPoint + 1) % PatrolPoints.Length;
                }
                return;
            }

            if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
            {
                _waiting = true;
                _waitCounter = WaitTime;
                _rb.linearVelocity = Vector2.zero;

                // Guardar rotaciones para interpolar
                _startRotation = transform.rotation;
                _targetRotation = _startRotation * Quaternion.Euler(0, 0, RotationAngle);
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

        //if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        //{
        //    _waiting = true;
        //    _waitCounter = WaitTime;
        //    _rb.linearVelocity = Vector2.zero;

        //    RotateTowards(transform, direction);
        //}
    }

    //private void RotateTowards(Transform transform, Vector2 direction)
    //{
    //    float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
    //    Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

    //    float rotationSpeed = 1f / Mathf.Max(WaitTime, 0.01f); // evita divisi�n por cero
    //    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    //}
}