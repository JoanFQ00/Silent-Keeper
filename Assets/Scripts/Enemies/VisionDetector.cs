using UnityEngine;

public class VisionDetector : MonoBehaviour
{
    public float VisionRange = 10f;
    public float VisionAngle = 60f;
    public LayerMask Walls;
    private Transform _player;
    private Transform _eyePosition;
    private SpriteRenderer _visionIndicator;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _eyePosition = transform;

        GameObject visionObject = new GameObject("VisionRange");
        visionObject.transform.SetParent(transform);
        visionObject.transform.localPosition = Vector3.zero;

        _visionIndicator = visionObject.AddComponent<SpriteRenderer>();
        _visionIndicator.sprite = Resources.Load<Sprite>("VisionCone");
        _visionIndicator.color = new Color(1, 1, 0, 0.3f); // Yellow
        _visionIndicator.sortingOrder = -1;
        _visionIndicator.transform.localScale = new Vector3(VisionRange * 0.2f, VisionRange * 0.2f, 1);
    }

    void Update()
    {
        if (CheckPlayerInVision())
        {
            //Debug.Log("Jugador detectado");
        }
    }

    private bool CheckPlayerInVision()
    {
        if (_player == null)
        {
            return false;
        }

        Vector2 directionToPlayer = (_player.position - _eyePosition.position).normalized;
        float distanceToPlayer = Vector2.Distance(_eyePosition.position, _player.position);

        if (distanceToPlayer <= VisionRange)
        {
            float angleToPlayer = Vector2.Angle(_eyePosition.right, directionToPlayer);
            if (angleToPlayer < VisionAngle / 2)
            {
                RaycastHit2D hit = Physics2D.Raycast(_eyePosition.position, directionToPlayer, distanceToPlayer, Walls);
                return hit.collider == null;
            }
        }
        return false;
    }

    void LateUpdate()
    {
        _visionIndicator.transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z);
    }
}