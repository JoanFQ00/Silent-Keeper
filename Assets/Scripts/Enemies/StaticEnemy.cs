using UnityEngine;

public class StaticEnemy : MonoBehaviour
{
    public float RotationAngle = 180f;
    public float RotationSpeed = 2f;
    public float WaitTime = 2f;

    private float _startAngle;
    private float _targetAngle;
    private float _waitCounter;
    private bool _waiting = false;
    private bool _rotatingForward = true;

    void Start()
    {
        _startAngle = transform.eulerAngles.z;
        _targetAngle = _startAngle + RotationAngle;
        _waitCounter = WaitTime;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isPaused)
            return;

        if (gameObject.GetComponent<EnemyDetector>().GetEnemyState() == EnemyDetector.EnemyState.NORMAL)
        {
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
}