using System;
using UnityEngine;

public class KidControl : MonoBehaviour
{
    GameObject kid;
    KidMachine kidMachine;

    void Start()
    {
        kid = GameObject.FindGameObjectWithTag("Kid");
        kidMachine = kid.GetComponent<KidMachine>();
    }

    void Update()
    {
        
    }

    private void OnMakeWaypoint() 
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

    private void OnFollowWay() 
    {
        kidMachine.ChangeState(KidMachine.States.Waypoints);
    }

    private void OnStopKid() 
    {
        kidMachine.StopResume();
    }
}
