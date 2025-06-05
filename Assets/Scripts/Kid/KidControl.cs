using UnityEngine;

public class KidControl : MonoBehaviour
{
    GameObject kid;
    KidMachine kidMachine;
    MonsterMove monsterMove;

    public static GameObject interactable;

    void Start()
    {
        kid = GameObject.FindGameObjectWithTag("Kid");
        monsterMove = GetComponent<MonsterMove>();
        kidMachine = kid.GetComponent<KidMachine>();
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isPaused)
            return;

        if (interactable != null)
        {
            kid.GetComponent<Interact>().Interacting(kid.tag, interactable);
        }

        if (kidMachine.GetCurrentState() == KidMachine.States.Hand)
        {
            Vector2 kidVelocity = (Vector2)kid.transform.position - kidMachine.LastPosition;

            if (kidVelocity.sqrMagnitude > 0.001f)
            {
                Vector3 monsterScale = monsterMove.transform.localScale;
                kid.transform.localScale = new Vector3(Mathf.Sign(monsterScale.x) * Mathf.Abs(kid.transform.localScale.x), kid.transform.localScale.y, kid.transform.localScale.z);
            }
        }
    }

    private void OnLeftMouse()
    {
        if (GameManager.Instance.isPaused)
            return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Collider2D hitInteractable = Physics2D.OverlapPoint(mousePos, LayerMask.GetMask("Interactable"));
        if (hitInteractable != null)
        {
            if (hitInteractable.CompareTag("CajaMovible"))
            {
                interactable = hitInteractable.gameObject;
                return;
            }
            else
            {
                interactable = hitInteractable.gameObject;
            }
        }

        Collider2D wallHit = Physics2D.OverlapPoint(mousePos, LayerMask.GetMask("Wall"));
        if (wallHit != null)
            return;

        kidMachine.SafeResetWaypoints();
        var waypoint = WaypointsPool.GetObject();
        if (waypoint != null && kidMachine.GetCurrentState() != KidMachine.States.HiddenInLocker)
        {
            waypoint.transform.position = mousePos;
            kidMachine.AddWaypoint(waypoint);
            kidMachine.ChangeState(KidMachine.States.Waypoints);
        }
    }


    private void OnRightMouse()
    {
        if (!GameManager.Instance.isPaused)
        {
            if (kidMachine.GetCurrentState() == KidMachine.States.Waypoints)
            {
                kidMachine.ChangeState(KidMachine.States.Stoped);
            }
            else if (kidMachine.GetCurrentState() != KidMachine.States.HiddenInLocker)
            {
                kidMachine.ChangeState(KidMachine.States.Waypoints);
            }
        }
    }

    private void OnStopResume()
    {
        if (!GameManager.Instance.isPaused)
        {
            kidMachine.SafeResetWaypoints();
            if (monsterMove.GetMonsterState() != KidMachine.States.HiddenInLocker && kidMachine.GetCurrentState() != KidMachine.States.HiddenInLocker)
                kidMachine.ChangeState(KidMachine.States.Following);
        }
    }

    private void OnResetWayPoints()
    {
        if (!GameManager.Instance.isPaused)
        {
            kidMachine.SafeResetWaypoints();
        }
    }
}