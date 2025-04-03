using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;

public class KidMachine : MonoBehaviour
{
    public enum States { Following, Waypoints, Stoped }
    private States lastState;
    private States currentState;
    [SerializeField] float speed = 1.5f;
    [SerializeField] float distToPlayer = 2.0f;
    private GameObject player;
    private List<GameObject> waypoints = new List<GameObject>();
    private int currentWaypointIndex = 0;
    
    void Start()
    {
        currentState = States.Following;
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        switch (currentState) 
        {
            case States.Following:

                if (Vector2.Distance(transform.position, player.transform.position) > distToPlayer) 
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                }

            break;

            case States.Waypoints:  

                if (waypoints.Count < 1) 
                {
                    ChangeState(States.Stoped); 
                }
                else  
                {
                    GameObject currentWaypoint = waypoints[currentWaypointIndex];

                    if (Vector2.Distance(transform.position, currentWaypoint.transform.position) > 0.1) 
                    {
                        transform.position = Vector2.MoveTowards(transform.position, currentWaypoint.transform.position, speed * Time.deltaTime);
                    }
                    else 
                    {
                        waypoints[currentWaypointIndex].SetActive(false);
                        currentWaypointIndex += 1;
 
                        if (currentWaypointIndex +1 > waypoints.Count) 
                        {
                            waypoints = new List<GameObject>();
                            currentWaypointIndex = 0;
                            WaypointsPool.DeactivateAll();
                            break;
                        }
                        
                        if (waypoints.Count != 0) 
                        {
                            currentWaypoint = waypoints[currentWaypointIndex];
                        }
                    }
                }

            break;

            case States.Stoped:
            break;
        }
    }

    public void ChangeState(States state) 
    {
        currentState = state;
    }

    public States GetCurrentState() 
    {
        return currentState;
    }

    public void StopResume() 
    {
        if (currentState != States.Stoped)
        {
            lastState = currentState;
            ChangeState(States.Stoped);
        }
        else 
        {
            if (lastState == States.Waypoints && waypoints.Count == 0) 
            {
                lastState = States.Following;
            }
            ChangeState(lastState);
        }
    }

    public void AddWaypoint(GameObject waypoint) 
    {
        waypoints.Add(waypoint);
    }

    public List<GameObject> GetWaypoints() 
    {
        return waypoints;
    }
}
