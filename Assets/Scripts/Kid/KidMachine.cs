using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System;

public class KidMachine : MonoBehaviour
{
    public enum States { Following, Hand, Waypoints, Stoped, Hidden }
    private States lastState;
    private States currentState;
    [SerializeField] float defaultSpeed = 1.5f;
    private float currentSpeed;
    private float distToPlayer = 0.7f;
    private GameObject player;
    private List<GameObject> waypoints;
    private int currentWaypointIndex = 0;
    
    void Start()
    {
        waypoints = new List<GameObject>();
        currentState = States.Following;
        player = GameObject.FindWithTag("Player");
        currentSpeed = defaultSpeed;
        //distToPlayer = GetComponent<Interact>().GetDetectionRadius();
    }

    void Update()
    {
        switch (currentState)
        {
            case States.Following:

                if (Vector2.Distance(gameObject.transform.position, player.transform.position) <= distToPlayer)
                {
                    ChangeState(States.Hand);
                }
                currentSpeed = defaultSpeed;
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentSpeed * Time.deltaTime);

                break;

            case States.Waypoints:

                currentSpeed = defaultSpeed;
                if (waypoints.Count < 1)
                {
                    ChangeState(States.Stoped);
                }
                else
                {
                    GameObject currentWaypoint = waypoints[currentWaypointIndex];

                    if (Vector2.Distance(transform.position, currentWaypoint.transform.position) >= 0.25f)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, currentWaypoint.transform.position, currentSpeed * Time.deltaTime);
                    }
                    else
                    {
                        waypoints[currentWaypointIndex].SetActive(false);
                        currentWaypointIndex += 1;

                        if (currentWaypointIndex + 1 > waypoints.Count)
                        {
                            waypoints = new List<GameObject>();
                            ResetCurrentWaypointIndex();
                            WaypointsPool.DeactivateAll();
                            break;
                        }
                    }
                }

                break;

            case States.Hand:

                if (distToPlayer < Vector2.Distance(player.transform.position, gameObject.transform.position))
                {
                    currentSpeed = player.GetComponent<MonsterMove>().GetSpeed();
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentSpeed * Time.deltaTime);
                }

                break;

            case States.Stoped:

                if (Vector2.Distance(player.transform.position, gameObject.transform.position) < distToPlayer)
                {
                    ChangeState(States.Hand);
                }

                break;

            default:
                break;
        }

        if (currentState == States.Hand && player.GetComponent<MonsterMove>().GetMonsterState() == States.Hidden)
        {
            currentState = States.Stoped;
        }
    }


    public void ChangeState(States state) 
    {
        currentState = state;

        if (currentState == States.Hand)
        {
         //   distToPlayer = GetComponent<Interact>().GetDetectionRadius();
        }
        else 
        {
            distToPlayer = 0.7f;
        }
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

    public void RemoveList() 
    {
        waypoints.Clear();
    }

    public void ResetCurrentWaypointIndex()
    {
        currentWaypointIndex = 0;
    }
}