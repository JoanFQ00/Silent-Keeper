using UnityEngine;
using UnityEngine.Rendering.Universal;
using static EnemyDetector;

public class PatrolBehaviour : MonoBehaviour
{
    public bool enemyPingPong = false;
    public float Speed = 3f;
    public float WaitTime = 2f;
    public float RotationTime = 1f;
    public Transform[] PatrolPoints;
    public bool enemyCircusAnim = false, isMonsterEnemy = false;

    private bool _movingForward;
    private int _currentPoint;
    private float _waitTimer;
    private float _rotationTimer;

    private bool waiting;
    private bool rotating;

    private Rigidbody2D _rb;
    private Transform targetPoint;

    private Vector2 direction;
    private Quaternion startRotation;
    private Quaternion targetRotation;

    private EnemyState patrolState;

    //private Light2D enemyLight;

    private void OnEnable()
    {
        IntroRoomsManager.OnCircusEnemyAnim += OnEnemyCircusCanMove;
    }

    private void OnDisable()
    {
        IntroRoomsManager.OnCircusEnemyAnim -= OnEnemyCircusCanMove;
    }

    private void OnEnemyCircusCanMove()
    {
        if (enemyCircusAnim)
            enemyCircusAnim = false;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        //enemyLight = GetComponentInChildren<Light2D>();

        _currentPoint = 0;
        transform.position = PatrolPoints[_currentPoint].position;

        _currentPoint = (_currentPoint + 1) % PatrolPoints.Length;
        targetPoint = PatrolPoints[_currentPoint];
        direction = (targetPoint.position - transform.position).normalized;

        patrolState = EnemyState.NORMAL;
        waiting = false;
        rotating = false;
        _movingForward = true;
    }

    void Update()
    {
        if (enemyCircusAnim)
        {
            if (GameManager.Instance.isPaused || IntroRoomsManager.Instance.roomInAnimation)
            {
                _rb.linearVelocity = Vector2.zero;
                return;
            }
        }
        else
            if (GameManager.Instance.isPaused)
            {
                _rb.linearVelocity = Vector2.zero;
                return;
            }

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

               //if (!isMonsterEnemy)
               //     StartRotation();

                StartRotation();
            }
            return;
        }

        if (rotating)
        {
            _rb.linearVelocity = Vector2.zero;
            _rotationTimer -= Time.deltaTime;

            if (!isMonsterEnemy)
            {
                float t = 1f - (_rotationTimer / RotationTime);
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            }

            if (_rotationTimer <= 0f)
            {
                rotating = false;

                //if (enemyLight != null)
                //    enemyLight.enabled = true;

                if (enemyPingPong)
                {
                    if (_movingForward)
                    {
                        _currentPoint++;
                        if (_currentPoint >= PatrolPoints.Length)
                        {
                            _currentPoint = PatrolPoints.Length - 2;
                            _movingForward = false;
                        }
                    }
                    else
                    {
                        _currentPoint--;
                        if (_currentPoint < 0)
                        {
                            _currentPoint = 1;
                            _movingForward = true;
                        }
                    }
                }
                else
                {
                    _currentPoint = (_currentPoint + 1) % PatrolPoints.Length;
                }

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

        //if (enemyLight != null)
        //    enemyLight.enabled = false;

        int nextIndex;

        if (enemyPingPong)
        {
            if (_movingForward)
            {
                nextIndex = _currentPoint + 1;
                if (nextIndex >= PatrolPoints.Length)
                    nextIndex = PatrolPoints.Length - 2;
            }
            else
            {
                nextIndex = _currentPoint - 1;
                if (nextIndex < 0)
                    nextIndex = 1;
            }
        }
        else
        {
            nextIndex = (_currentPoint + 1) % PatrolPoints.Length;
        }

        Vector3 newDirection = (PatrolPoints[nextIndex].position - transform.position).normalized;
        float signedAngle = Vector3.SignedAngle(direction, newDirection, Vector3.forward);
        targetRotation = startRotation * Quaternion.Euler(0, 0, signedAngle);
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

    public void ResetCurrentPoint()
    {
        waiting = false;
        rotating = false;
        _waitTimer = 0f;
        _rotationTimer = 0f;

        patrolState = EnemyState.NORMAL;
        GetComponent<EnemyDetector>().SetEnemyState(EnemyState.NORMAL);

        _movingForward = true;

        _currentPoint = 0;
        transform.position = PatrolPoints[_currentPoint].position;
        transform.rotation = startRotation;

        _currentPoint = (_currentPoint + 1) % PatrolPoints.Length;
        targetPoint = PatrolPoints[_currentPoint];
        direction = (targetPoint.position - transform.position).normalized;
    }
}