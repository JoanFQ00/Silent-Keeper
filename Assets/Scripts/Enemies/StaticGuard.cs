using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;



public class StaticGuard : MonoBehaviour
{
    public LayerMask Walls;
    //public Transform spawnerPosition;

    private Transform _player;
    private Transform _kid;

    private Light2D _light2D;

    private float VisionRange;
    private float VisionAngle;

    public enum Characters
    {
        BOTH,
        ONE
    }

    public Characters enemyState;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        _kid = GameObject.FindGameObjectWithTag("Kid")?.transform;

        _light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        if (_light2D != null)
        {
            // Actualiza los valores desde la luz
            VisionRange = _light2D.pointLightOuterRadius;
            VisionAngle = _light2D.pointLightOuterAngle;
        }

        if (enemyState == Characters.BOTH)
        {
            if (CheckTargetInVision(_player) || CheckTargetInVision(_kid))
                SendToSpawner();
        }
        else
        {
            if (CheckTargetInVision(_kid))
                SendToSpawner();
        }
    }

    //private bool CheckTargetInVision(Transform target)
    //{
    //    if (target == null) return false;

    //    Vector2 directionToTarget = (target.position - transform.position).normalized;
    //    float distanceToTarget = Vector2.Distance(transform.position, target.position);

    //    if (distanceToTarget <= VisionRange)
    //    {
    //        float angleToTarget = Vector2.Angle(transform.up, directionToTarget);
    //        if (angleToTarget < VisionAngle / 2f)
    //        {
    //            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, Walls);
    //            return hit.collider == null || hit.collider.transform == target;
    //        }
    //    }

    //    return false;
    //}

    private bool CheckTargetInVision(Transform target)
    {
        if (target == null) return false;

        Vector2 directionToTarget = (target.position - transform.position).normalized;
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget <= VisionRange)
        {
            float angleToTarget = Vector2.Angle(transform.up, directionToTarget);
            if (angleToTarget < VisionAngle / 2f)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, Walls);

                // Si no hay colisión, o la colisión es el objetivo Y no es un trigger
                if (hit.collider == null)
                    return true;

                // Si el collider es un trigger, lo ignoramos
                if (hit.collider.isTrigger)
                    return true;

                // Solo permitir si lo que choca es directamente el target (y no hay nada en medio)
                return hit.collider.transform == target;
            }
        }

        return false;
    }

    private void SendToSpawner()
    {
        if (_light2D != null)
            _light2D.color = Color.red;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //private void SendToSpawner()
    //{
    //    gameObject.GetComponent<Light2D>().color = Color.red;
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //    if (SpawnerPosition != null)
    //    {
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //        //target.position = SpawnerPosition.position;
    //    }
    //}

    private void OnDrawGizmos()
    {
        Light2D light = GetComponent<Light2D>();
        if (light == null) return;

        float range = light.pointLightOuterRadius;
        float angle = light.pointLightOuterAngle;

        Vector3 origin = transform.position;
        Vector3 forward = transform.up;

        // Dibuja el círculo del rango
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origin, range);

        // Calcula los límites del ángulo de visión
        Quaternion leftRotation = Quaternion.Euler(0, 0, -angle / 2f);
        Quaternion rightRotation = Quaternion.Euler(0, 0, angle / 2f);

        Vector3 leftDir = leftRotation * forward;
        Vector3 rightDir = rightRotation * forward;

        // Dibuja las dos líneas del ángulo
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + leftDir * range);
        Gizmos.DrawLine(origin, origin + rightDir * range);
    }
}