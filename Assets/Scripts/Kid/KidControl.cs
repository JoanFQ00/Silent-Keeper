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
        if (monsterMove.currentMode == MonsterMove.Mode.KidCon)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
        if (monsterMove.currentMode == MonsterMove.Mode.KidCon)
        {
            kidMachine.ChangeState(KidMachine.States.Waypoints);
        }
    }

    private void OnStopKid() 
    {
        kidMachine.StopResume();
    }
}
