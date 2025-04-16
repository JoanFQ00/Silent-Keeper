using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StaticEnemy : MonoBehaviour
{
    public float RotationAngle = 180f;
    public float RotationSpeed = 2f;
    public float WaitTime = 2f;
    public LayerMask Walls;

    private Light2D _light2D;
    private Transform _player;
    private Transform _kid;

    private float _startAngle;
    private float _targetAngle;
    private float _waitCounter;
    private bool _waiting = false;
    private bool _rotatingForward = true;

    private enum EnemyState { NORMAL, STUNNED }
    private EnemyState enemyState;

    void Start()
    {
        enemyState = EnemyState.NORMAL;

        _light2D = GetComponent<Light2D>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        _kid = GameObject.FindGameObjectWithTag("Kid")?.transform;

        _startAngle = transform.eulerAngles.z;
        _targetAngle = _startAngle + RotationAngle;
        _waitCounter = WaitTime;
    }

    void Update()
    {
        if (enemyState != EnemyState.NORMAL) return;

        if (_waiting)
        {
            _waitCounter -= Time.deltaTime;
            if (_waitCounter <= 0f)
            {
                _waiting = false;
                _waitCounter = WaitTime;

                _rotatingForward = !_rotatingForward;

                if (_rotatingForward)
                {
                    _targetAngle = _startAngle + RotationAngle;
                }
                else
                {
                    _targetAngle = _startAngle;
                }
            }
        }
        else
        {
            RotateToAngle();
        }

        if (CheckTargetInVision(_player) || CheckTargetInVision(_kid))
        {
            SendToSpawner();
        }
    }

    void RotateToAngle()
    {
        Quaternion goalRotation = Quaternion.Euler(0f, 0f, _targetAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, goalRotation, RotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, goalRotation) < 1f)
        {
            _waiting = true;
        }
    }

    bool CheckTargetInVision(Transform target)
    {
        if (target == null) return false;

        Vector2 dirToTarget = (target.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, target.position);
        float visionRange = _light2D.pointLightOuterRadius;
        float visionAngle = _light2D.pointLightOuterAngle;

        if (distance <= visionRange)
        {
            float angle = Vector2.Angle(transform.up, dirToTarget);

            if (angle < visionAngle / 2f)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToTarget, distance, Walls);

                if (target.CompareTag("Player"))
                {
                    var playerScript = target.GetComponent<MonsterMove>();
                    if (playerScript != null && playerScript.GetMonsterState() != KidMachine.States.Hidden)
                    {
                        if (hit.collider == null || hit.collider.isTrigger || hit.collider.transform == target)
                            return true;
                    }
                }
                else if (target.CompareTag("Kid"))
                {
                    var kidScript = target.GetComponent<KidMachine>();
                    if (kidScript != null && kidScript.GetCurrentState() != KidMachine.States.Hidden)
                    {
                        if (hit.collider == null || hit.collider.isTrigger || hit.collider.transform == target)
                            return true;
                    }
                }
            }
        }

        return false;
    }

    void SendToSpawner()
    {
        if (_light2D != null)
        {
            _light2D.color = Color.red;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void OnDrawGizmos()
    {
        if (_light2D == null) return;

        float range = _light2D.pointLightOuterRadius;
        float angle = _light2D.pointLightOuterAngle;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);

        Vector3 forward = transform.up;
        Vector3 leftDir = Quaternion.Euler(0, 0, -angle / 2f) * forward;
        Vector3 rightDir = Quaternion.Euler(0, 0, angle / 2f) * forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * range);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * range);
    }
}