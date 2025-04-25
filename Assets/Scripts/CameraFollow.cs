using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothTime = 0.25f;
    public float offsetFactor = 0.3f;
    public float maxOffsetDistance = 5f;
    public float deadZone = 2f;

    private Transform target;
    private Vector3 velocity = Vector3.zero;
    private Camera mainCamera;
    private bool cameraReturned;

    private void Start()
    {
        GameManager.Instance.SetInitialCameraReference(gameObject);

        transform.position = new Vector3(target.position.x, target.position.y, -10f);
        mainCamera = GetComponent<Camera>();
        mainCamera.orthographicSize = 5f;
        cameraReturned = true;
    }

    void Update()
    {
        if (!GameManager.Instance.isPaused && target != null) 
        {
            if (target.gameObject.tag == "Player")
                if (target.gameObject.GetComponent<Rigidbody2D>().linearVelocity.sqrMagnitude < 0.01f && cameraReturned)
                    CameraMove();
                else ReturnToTarget();
            else if (target.gameObject.tag == "Kid")
                if (cameraReturned && (target.gameObject.GetComponent<KidMachine>().GetCurrentState() == KidMachine.States.Stoped || target.gameObject.GetComponent<KidMachine>().GetCurrentState() == KidMachine.States.HiddenInLocker 
                || GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().linearVelocity.sqrMagnitude < 0.01f && target.gameObject.GetComponent<KidMachine>().GetCurrentState() == KidMachine.States.Hand))
                    CameraMove();
                else ReturnToTarget();
            else
            {
                ReturnToTarget();
            }
        }
    }

    private void CameraMove()
    {
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
        targetPosition.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    private void ReturnToTarget()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10f);
        float lerpSpeed = 8f;

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            cameraReturned = true;
        }
        else
        {
            cameraReturned = false;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
        }
    }

    public Transform GetCameraTarget()
    {
        return target;
    }

    public void ChangeTarget(Transform _target)
    {
        target = _target;
        ReturnToTarget();
    }
}