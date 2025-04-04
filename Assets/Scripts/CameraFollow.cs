using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform target;
    private float smoothTime = 0.25f;
    Vector3 velocity = Vector3.zero;

    Camera mainCamera;

    private void Start()
    {
        transform.position = target.position;
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (target.tag == "Player") 
        {
            mainCamera.orthographicSize = 5f;
        }
        else 
        {
            mainCamera.orthographicSize = 3.5f;
        }

        Vector3 targetPosition = target.position;
        targetPosition.z = -1;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void ChangeTarget(Transform _target)
    {
        target = _target;
    }
}
