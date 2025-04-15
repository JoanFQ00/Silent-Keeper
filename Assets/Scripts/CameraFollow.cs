using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform target;
    public float smoothTime = 0.25f;
    public float offsetFactor = 0.3f;
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

        Vector3 mouseScreen = Input.mousePosition;
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, mainCamera.nearClipPlane));

        Vector3 direction = (mouseWorld - target.position);
        direction.z = 0f;

        Vector3 offset = new Vector3(direction.x * offsetFactor, direction.y * offsetFactor, 0f);

        Vector3 targetPos = target.position + offset;
        targetPos.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothTime * Time.deltaTime);
    }

    public void ChangeTarget(Transform _target)
    {
        target = _target;
    }
}
