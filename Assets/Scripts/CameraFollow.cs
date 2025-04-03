using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform target;
    private float smoothTime = 0.25f;
    Vector3 velocity = Vector3.zero;

    private void Start()
    {
        transform.position = target.position;
    }


    void Update()
    {
        Vector3 targetPosition = target.position;
        targetPosition.z = -1;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void ChangeTarget(Transform _target)
    {
        target = _target;
    }
}
