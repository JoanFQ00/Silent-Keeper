using Unity.VisualScripting;
using UnityEngine;

public class KidMachine : MonoBehaviour
{
    public enum States { Following, Waypoints, Stoped }
    private States lastState;
    private States currentState;

    [SerializeField] float speed = 1.5f;
    [SerializeField] float distToPlayer = 2.0f;
    private GameObject player;

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

            break;

            case States.Stoped:

            break;
        }
    }

    public void ChangeState(States state) 
    {
        currentState = state;
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
            ChangeState(lastState);
        }
    }
}
