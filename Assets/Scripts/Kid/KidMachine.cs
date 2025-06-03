using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;

public class KidMachine : MonoBehaviour
{
    [SerializeField] float defaultSpeed = 1.5f;
    [SerializeField] float followingDistance = 0.7f, distToPlayer = 0.5f;

    // [SerializeField]
    // public AudioSource footstepsAudio;
    // [SerializeField]
    // [Range(0f, 1f)] private float footstepsVolume = 0.5f;

    [HideInInspector] public int bushCounter = 0;
    public Color originalColor;

    public enum States { Following, Hand, Waypoints, Stoped, HiddenInLocker, HiddenInBush, NotHiddenInBush }

    private States currentState, lastState, hiddenState;
    private GameObject player;
    private List<GameObject> waypoints = new List<GameObject>();
    private int currentWaypointIndex = 0;
    private LayerMask wallMask, interactableMask;
    private float currentSpeed;

    private Animator kidAnimator;
    private Vector2 lastPosition, lastNonZeroDir = Vector2.down;
    private Vector3 initialScale;
    private Transform feetTransform;

    public Vector2 LastPosition => lastPosition;

    void Start()
    {
        currentState = States.Following;
        hiddenState = States.NotHiddenInBush;

        originalColor = GetComponent<SpriteRenderer>().color;
        feetTransform = transform.GetChild(0);

        player = GameObject.FindWithTag("Player");
        wallMask = LayerMask.GetMask("Wall");
        interactableMask = LayerMask.GetMask("Interactable");

        currentSpeed = defaultSpeed;

        kidAnimator = GetComponent<Animator>();
        lastPosition = transform.position;
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (GameManager.Instance.isPaused || IntroRoomsManager.Instance.roomInAnimation)
        {
            UpdateAnimator(Vector2.zero);
            // StopFootstepAudio();
            return;
        }

        switch (currentState)
        {
            case States.Following:
                HandleFollowingState();
                break;
            case States.Hand:
                HandleHandState();
                break;
            case States.Waypoints:
                HandleWaypointsState();
                break;
            case States.Stoped:
                HandleStoppedState();
                break;
        }

        if (currentState == States.Hand && player.GetComponent<MonsterMove>().GetMonsterState() == States.HiddenInLocker)
        {
            ChangeState(States.Stoped);
        }

        Vector2 moveDelta = (Vector2)transform.position - lastPosition;
        if (currentState == States.Stoped || moveDelta.magnitude < 0.001f)
        {
            moveDelta = Vector2.zero;
        }
        UpdateAnimator(moveDelta);
        // HandleFootstepAudio();

        lastPosition = transform.position;
    }

    private void UpdateAnimator(Vector2 moveDelta)
    {
        if (kidAnimator == null) return;

        if (Mathf.Abs(moveDelta.x) > 0.01f)
        {
            transform.localScale = new Vector3(moveDelta.x < 0 ? -initialScale.x : initialScale.x, initialScale.y, initialScale.z);
        }

        if (moveDelta.sqrMagnitude > 0.0001f)
        {
            lastNonZeroDir = moveDelta.normalized;
        }

        kidAnimator.SetFloat("Horizontal", lastNonZeroDir.x);
        kidAnimator.SetFloat("Vertical", lastNonZeroDir.y);
        kidAnimator.SetBool("Speed", moveDelta.sqrMagnitude > 0.00001f);
    }

    private void HandleFollowingState()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= distToPlayer)
        {
            ChangeState(States.Hand);
        }
        else
        {
            currentSpeed = defaultSpeed;
            if (!IsWallBetween(transform, player.transform) && !IsInteractableWithLocker(transform, player.transform))
            {
                MoveTowards(player.transform);
            }
            else
            {
                ChangeState(States.Stoped);
            }
        }
    }

    private void HandleHandState()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > distToPlayer)
        {
            currentSpeed = player.GetComponent<MonsterMove>().GetSpeed();
            if (!IsWallBetween(transform, player.transform) && !IsInteractableWithLocker(transform, player.transform))
            {
                MoveTowards(player.transform);
            }
        }
    }

    private void HandleWaypointsState()
    {
        currentSpeed = defaultSpeed;

        if (waypoints.Count < 1 || currentWaypointIndex >= waypoints.Count)
        {
            ChangeState(States.Stoped);
            return;
        }

        GameObject currentWaypoint = waypoints[currentWaypointIndex];

        if (!IsWallBetween(transform, currentWaypoint.transform))
        {
            if (Vector2.Distance(feetTransform.position, currentWaypoint.transform.position) >= 0.25f)
            {
                MoveTowardsFoot(currentWaypoint.transform);
            }
            else
            {
                currentWaypoint.SetActive(false);
                currentWaypointIndex++;

                if (currentWaypointIndex >= waypoints.Count)
                {
                    waypoints.Clear();
                    ResetCurrentWaypointIndex();
                    WaypointsPool.DeactivateAll();
                    ChangeState(States.Stoped);
                }
            }
        }
        else
        {
            if (currentWaypointIndex > 0)
                ChangeState(States.Stoped);
            else if (lastState != States.Stoped)
                ChangeState(States.Hand);
        }
    }

    private void HandleStoppedState()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < followingDistance &&
            !IsWallBetween(transform, player.transform) && !IsInteractableWithLocker(transform, player.transform))
        {
            ChangeState(States.Hand);
        }
    }

    private void MoveTowardsFoot(Transform target)
    {
        Vector3 offset = transform.position - feetTransform.position;
        Vector3 newFeetPos = Vector3.MoveTowards(feetTransform.position, target.position, currentSpeed * Time.deltaTime);

        transform.position = newFeetPos + offset;
    }

    private void MoveTowards(Transform target)
    {
        Vector2 newPos = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
        transform.position = newPos;
    }

    private bool IsWallBetween(Transform from, Transform to)
    {
        Vector2 direction = (to.position - from.position).normalized;
        float distance = Vector2.Distance(from.position, to.position);
        return Physics2D.Raycast(from.position, direction, distance, wallMask);
    }

    private bool IsInteractableWithLocker(Transform from, Transform to)
    {
        return false;
    }

    public void ChangeState(States state)
    {
        lastState = currentState;
        currentState = state;
    }

    public States GetCurrentState()
    {
        return currentState;
    }

    public void AddWaypoint(GameObject waypoint)
    {
        waypoints.Add(waypoint);
    }

    public void RemoveList()
    {
        waypoints.Clear();
    }

    public void ResetCurrentWaypointIndex()
    {
        currentWaypointIndex = 0;
    }

    public void SafeResetWaypoints()
    {
        RemoveList();
        ResetCurrentWaypointIndex();
        WaypointsPool.DeactivateAll();

        if (currentState != States.Hand && currentState != States.Following && currentState != States.HiddenInLocker)
        {
            ChangeState(States.Stoped);
        }
    }

    public void ChangeHiddenState(States state)
    {
        hiddenState = state;
    }

    public States GetHiddenKidState()
    {
        return hiddenState;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distToPlayer);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, followingDistance);
    }

    private void PushBackFromCollision(float pushDistance = 0.1f)
    {
        Vector3 pushDir = -((Vector3)lastNonZeroDir).normalized;
        transform.position += pushDir * pushDistance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layer = collision.gameObject.layer;

        if (((1 << layer) & interactableMask) != 0)
        {
            ChangeState(States.Stoped);
            PushBackFromCollision();
        }
        else if (((1 << layer) & wallMask) != 0)
        {
            if (currentState != States.Stoped)
            {
                ChangeState(States.Stoped);
                PushBackFromCollision(0.01f);
            }
        }
    }

    // private void HandleFootstepAudio()
    // {
    //     bool shouldPlay = (currentState == States.Following || currentState == States.Hand || currentState == States.Waypoints);
    //     bool isMoving = Vector2.Distance(transform.position, lastPosition) > 0.005f;

    //     if (shouldPlay && isMoving)
    //     {
    //         if (footstepsAudio != null && !footstepsAudio.isPlaying)
    //         {
    //             footstepsAudio.volume = footstepsVolume;
    //             footstepsAudio.Play();
    //         }
    //     }
    //     else
    //     {
    //         StopFootstepAudio();
    //     }
    // }

    // private void StopFootstepAudio()
    // {
    //     if (footstepsAudio != null && footstepsAudio.isPlaying)
    //     {
    //         footstepsAudio.Stop();
    //     }
    // }
}