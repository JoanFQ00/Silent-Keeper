using UnityEngine;

public class KidControl : MonoBehaviour
{
    GameObject kid;
    KidMachine kidMachine;
    MonsterMove monsterMove;

    void Start()
    {
        kid = GameObject.FindGameObjectWithTag("Kid");
        monsterMove = GetComponent<MonsterMove>();
        kidMachine = kid.GetComponent<KidMachine>();
    }

    private void OnLeftMouse()
    {
        if (!GameManager.Instance.isPaused)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            float wallMargin = 0.3f;
            Collider2D nearWall = Physics2D.OverlapCircle(mousePos, wallMargin, LayerMask.GetMask("Wall"));

            if (nearWall == null)
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider == null || hit.collider.isTrigger)
                {
                    var waypoint = WaypointsPool.GetObject();
                    if (waypoint != null)
                    {
                        waypoint.transform.position = mousePos;
                        kidMachine.AddWaypoint(waypoint);
                    }
                }
            }
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