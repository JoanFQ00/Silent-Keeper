using UnityEngine;

public class VisionDetection : MonoBehaviour
{
    public float VisionRange = 10f;
    public float VisionAngle = 60f;
    public LayerMask Walls;
    public Transform SpawnerPosition;

    private Transform _eyePosition;
    private SpriteRenderer _visionIndicator;

    private Transform _player;
    private Transform _kid;

    void Start()
    {
        _eyePosition = transform;

        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        _kid = GameObject.FindGameObjectWithTag("Kid")?.transform;

        GameObject visionObject = new GameObject("VisionRange");
        visionObject.transform.SetParent(transform);
        visionObject.transform.localPosition = Vector3.zero;

        _visionIndicator = visionObject.AddComponent<SpriteRenderer>();
        _visionIndicator.color = new Color(1, 1, 0, 0.3f);
        _visionIndicator.sortingOrder = -1;
        _visionIndicator.transform.localScale = new Vector3(VisionRange * 0.2f, VisionRange * 0.2f, 1);
    }

    void Update()
    {
        if (CheckTargetInVision(_player))
        {
            SendToSpawner(_player);
        }

        if (CheckTargetInVision(_kid))
        {
            SendToSpawner(_kid);
        }
    }

    private bool CheckTargetInVision(Transform target)
    {
        if (target == null) return false;

        Vector2 directionToTarget = (target.position - _eyePosition.position).normalized;
        float distanceToTarget = Vector2.Distance(_eyePosition.position, target.position);

        // Línea visual para debug
        Debug.DrawLine(_eyePosition.position, target.position, Color.red);

        if (distanceToTarget <= VisionRange)
        {
            float angleToTarget = Vector2.Angle(_eyePosition.right, directionToTarget);
            if (angleToTarget < VisionAngle / 2)
            {
                RaycastHit2D hit = Physics2D.Raycast(_eyePosition.position, directionToTarget, distanceToTarget, Walls);
                if (hit.collider == null)
                {
                    return true; //No hay obstáculos
                }
                else
                {
                    //Si el obstáculo es el propio objetivo
                    return hit.collider.transform == target;
                }
            }
        }
        return false;
    }

    private void SendToSpawner(Transform target)
    {
        if (SpawnerPosition != null)
        {
            target.position = SpawnerPosition.position;
        }
    }

    void LateUpdate()
    {
        _visionIndicator.transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z);
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            _eyePosition = transform;

        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(_eyePosition.position, VisionRange);

        Vector3 rightDirection = Quaternion.Euler(0, 0, VisionAngle / 2) * _eyePosition.right;
        Vector3 leftDirection = Quaternion.Euler(0, 0, -VisionAngle / 2) * _eyePosition.right;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_eyePosition.position, _eyePosition.position + rightDirection * VisionRange);
        Gizmos.DrawLine(_eyePosition.position, _eyePosition.position + leftDirection * VisionRange);
    }
}
