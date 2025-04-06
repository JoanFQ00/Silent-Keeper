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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (monsterMove.GetCurrentMode() == MonsterMove.Mode.KidCon)
        {
            //mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0.0f;

            var waypoint = WaypointsPool.GetObject();

            if (waypoint != null)
            {
                waypoint.transform.position = mousePos;
                kidMachine.AddWaypoint(waypoint);
            }
        }
    }

    private void OnRightMouse() 
    {
        if (monsterMove.GetCurrentMode() == MonsterMove.Mode.KidCon)
        {
            if(kidMachine.GetCurrentState() == KidMachine.States.Waypoints) 
            {
                kidMachine.ChangeState(KidMachine.States.Stoped);
            }
            else 
            {
                kidMachine.ChangeState(KidMachine.States.Waypoints);
            }
        }
    }
}
