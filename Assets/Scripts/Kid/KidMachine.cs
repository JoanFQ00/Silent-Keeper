using UnityEngine;
using System.Collections.Generic;

public class KidMachine : MonoBehaviour
{
    [SerializeField] float defaultSpeed = 1.5f;
    [SerializeField] float followingDistance = 0.7f, distToPlayer = 0.5f;

    [HideInInspector]
    public int bushCounter = 0;
    public Color originalColor;

    public enum States { Following, Hand, Waypoints, Stoped, HiddenInLocker, HiddenInBush, NotHiddenInBush }

    private States currentState, lastState, hiddenState;
    private GameObject player;
    private List<GameObject> waypoints = new List<GameObject>();
    private int currentWaypointIndex = 0;
    private LayerMask wallMask;
    private float currentSpeed;

    private Animator kidAnimator;
    private Vector2 lastPosition;

    void Start()
    {
        currentState = States.Following;
        hiddenState = States.NotHiddenInBush;

        originalColor = GetComponent<SpriteRenderer>().color;

        player = GameObject.FindWithTag("Player");
        wallMask = LayerMask.GetMask("Wall");

        currentSpeed = defaultSpeed;

        kidAnimator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        if (GameManager.Instance.isPaused)
        {
            UpdateAnimator(Vector2.zero);
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
        UpdateAnimator(moveDelta);

        lastPosition = transform.position;
    }

    private void UpdateAnimator(Vector2 moveDelta)
    {
        if (kidAnimator == null) return;

        kidAnimator.SetFloat("Horizontal", moveDelta.x);
        kidAnimator.SetFloat("Vertical", moveDelta.y);
        kidAnimator.SetFloat("Speed", moveDelta.sqrMagnitude);
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
            if (!IsWallBetween(transform, player.transform))
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
            if (!IsWallBetween(transform, player.transform))
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
            if (Vector2.Distance(transform.position, currentWaypoint.transform.position) >= 0.25f)
            {
                MoveTowards(currentWaypoint.transform);
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
            !IsWallBetween(transform, player.transform))
        {
            ChangeState(States.Hand);
        }
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

        if (currentState != States.Hand && currentState != States.Following)
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
}