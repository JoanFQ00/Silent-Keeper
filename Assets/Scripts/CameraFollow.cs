using TMPro;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform target;
    public float smoothTime = 0.25f;
    public float offsetFactor = 0.3f;
    public float maxOffsetDistance = 5f;
    public float deadZone = 2f;
    private Vector3 velocity = Vector3.zero;

    Camera mainCamera;

    private void Start()
    {
        transform.position = target.position;
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (!GameManager.Instance.isPaused) 
        {
            if (target.tag == "Player")
            {
                mainCamera.orthographicSize = 5f;
            }
            else
            {
                mainCamera.orthographicSize = 3.5f;
            }

            Vector3 mouseScreen = Input.mousePosition;
            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, mainCamera.transform.position.z * -1));

            Vector3 direction = mouseWorld - target.position;
            direction.z = 0;

            Vector3 offset = Vector3.zero;
            if (direction.magnitude > deadZone)
            {
                Vector3 normalizedDir = direction.normalized;
                float effectiveDistance = direction.magnitude - deadZone;
                offset = normalizedDir * effectiveDistance * offsetFactor;
            }
            Vector3 clampedOffset = Vector3.ClampMagnitude(offset, maxOffsetDistance);

            Vector3 targetPosition = target.position + clampedOffset;
            targetPosition.z = -1;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    public void ChangeTarget(Transform _target)
    {
        target = _target;
    }
}
