using UnityEngine;
using static HeadHUD;

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
    private Collider2D roomCollider;
    private bool checkPlayersInCam;
    private bool playerInCam = false;
    private bool kidInCam = false;

    public static CameraFollow Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        checkPlayersInCam = true;
        ResetTarget();

        transform.position = new Vector3(target.position.x, target.position.y, -10f);
        mainCamera = GetComponent<Camera>();
        mainCamera.orthographicSize = 6.3f;
        cameraReturned = true;
    }

    void Update()
    {
        if (!GameManager.Instance.isPaused && target != null && !TransitionCamAnimation.Instance.playersInTransition && mainCamera != null)
        {
            if (!cameraReturned)
            {
                ReturnToTarget();
                return;
            }

            if (target.gameObject.tag == "Player")
            {
                if (target.gameObject.GetComponent<Rigidbody2D>().linearVelocity.sqrMagnitude < 0.01f && cameraReturned)
                    FreeMove();
                else
                    NormalMovement();
            }
            else if (target.gameObject.tag == "Kid")
            {
                var kidState = target.gameObject.GetComponent<KidMachine>().GetCurrentState();
                var playerStill = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().linearVelocity.sqrMagnitude < 0.01f;

                if (cameraReturned && (kidState == KidMachine.States.Stoped || kidState == KidMachine.States.HiddenInLocker
                    || (playerStill && kidState == KidMachine.States.Hand)))
                    FreeMove();
                else
                    NormalMovement();
            }
            else
            {
                ReturnToTarget();
            }
        }
    }

    private void FreeMove()
    {
        Vector3 mouseScreen = Input.mousePosition;
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, -mainCamera.transform.position.z));

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

    private void NormalMovement()
    {
        if (target == null || mainCamera == null)
            return;

        Vector3 playerPos = target.position;
        Vector3 camPos = mainCamera.transform.position;

        Vector3 targetPos = playerPos;

        if (roomCollider != null)
        {
            Bounds bounds = roomCollider.bounds;

            float halfHeight = mainCamera.orthographicSize;
            float halfWidth = halfHeight * mainCamera.aspect;

            float minX = bounds.min.x + halfWidth;
            float maxX = bounds.max.x - halfWidth;
            float minY = bounds.min.y + halfHeight;
            float maxY = bounds.max.y - halfHeight;

            targetPos = camPos;

            if (bounds.size.x >= halfWidth * 2)
            {
                targetPos.x = Mathf.Clamp(playerPos.x, minX, maxX);
            }
            else
            {
                targetPos.x = bounds.center.x;
            }

            if (bounds.size.y >= halfHeight * 2)
            {
                targetPos.y = Mathf.Clamp(playerPos.y, minY, maxY);
            }
            else
            {
                targetPos.y = bounds.center.y;
            }
        }

        Vector3 smoothTarget = new Vector3(targetPos.x, targetPos.y, camPos.z);
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, smoothTarget, ref velocity, smoothTime);
    }

    private void ReturnToTarget()
    {
        if (CameraFollow.Instance != null && target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10f);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                cameraReturned = true;
            }
            else
            {
                cameraReturned = false;
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime * 0.5f);
            }
        }
    }

    public void SetRoomCollider(Collider2D collider)
    {
        roomCollider = collider;
    }

    public Transform GetCameraTarget()
    {
        return target;
    }

    public void ResetTarget()
    {
        GameObject monster = GameObject.FindGameObjectWithTag("Player");
        ChangeTarget(monster.transform);

        if (HeadHUD.Instance != null)
            HeadHUD.Instance.OnFocusChanged(FocusTarget.Player1);
    }

    public void ChangeTarget(Transform _target)
    {
        target = _target;
        ReturnToTarget();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInCam = true;

        if (collision.CompareTag("Kid"))
            kidInCam = true;

        if (playerInCam && kidInCam && !checkPlayersInCam)
            checkPlayersInCam = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInCam = false;

        if (collision.CompareTag("Kid"))
            kidInCam = false;

        if ((!playerInCam || !kidInCam) && checkPlayersInCam)
            checkPlayersInCam = false;
    }

    public bool CheckPlayersInCam()
    {
        return checkPlayersInCam;
    }
}