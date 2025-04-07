using UnityEngine;

public class StaticGuard : MonoBehaviour
{
    public float VisionRange = 10f;
    public float VisionAngle = 60f;
    public Transform SpawnerPosition;
    public LayerMask Walls;

    private Transform _player;
    private Transform _kid;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        _kid = GameObject.FindGameObjectWithTag("Kid")?.transform;
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

        Vector2 directionToTarget = (target.position - transform.position).normalized;
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget <= VisionRange)
        {
            float angleToTarget = Vector2.Angle(transform.right, directionToTarget);
            if (angleToTarget < VisionAngle / 2)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, Walls);
                return hit.collider == null || hit.collider.transform == target;
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
}
