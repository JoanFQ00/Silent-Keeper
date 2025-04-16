using System;
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

            Vector3 origin = kid.transform.position;
            Vector3 direction = (mousePos - origin).normalized;
            float distance = Vector2.Distance(origin, mousePos);

            RaycastHit2D hitWall = Physics2D.Raycast(origin, direction, distance, LayerMask.GetMask("Wall"));

            if (hitWall.collider == null)
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
            else
            {
                kidMachine.ChangeState(KidMachine.States.Waypoints);
            }
        }
    }

    private void OnStopResume()
    {
        if (!GameManager.Instance.isPaused)
        {
            if (monsterMove.GetMonsterState() != KidMachine.States.Hidden && kidMachine.GetCurrentState() != KidMachine.States.Hidden)
            {
                kidMachine.ChangeState(KidMachine.States.Following);
                WaypointsPool.DeactivateAll();
                kidMachine.RemoveList();
                kidMachine.ResetCurrentWaypointIndex();
            }
        }
    }
}