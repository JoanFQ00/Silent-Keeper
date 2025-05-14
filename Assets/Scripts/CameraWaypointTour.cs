// using UnityEngine;
// using System.Collections.Generic;

// public class CameraWaypointTour : MonoBehaviour
// {
//     public float moveSpeed = 3f;
//     public List<Transform> waypoints;
//     private int currentIndex = 0;
//     private bool isTourActive = false;
//     private CameraFollow cameraFollow;

//     void Start()
//     {
//         cameraFollow = GetComponent<CameraFollow>();
//         if (waypoints != null && waypoints.Count > 0)
//         {
//             StartTour(waypoints);
//         }
//     }

//     void Update()
//     {
//         if (isTourActive && waypoints != null && currentIndex < waypoints.Count)
//         {

//         }
//     }

//     public void StartTour(List<Transform> tourPoints)
//     {
//         if (tourPoints == null || tourPoints.Count == 0)
//         {
//             return;
//         }

//         waypoints = tourPoints;
//         currentIndex = 0;
//         isTourActive = true;

//         GameManager.Instance.isPaused = true;
//         GameManager.Instance.StopCounting();

//         ReturnCharacters(true);

//         cameraFollow.enabled = false;
//     }

//     void EndTour()
//     {
//         isTourActive = false;

//         cameraFollow.enabled = true;

//         GameManager.Instance.isPaused = false;
//         GameManager.Instance.RestartTime();

//         ReturnCharacters(true);
//     }

//     void ReturnCharacters(bool enable)
//     {
//         GameObject kid = GameObject.FindGameObjectWithTag("Kid");
//         GameObject monster = GameObject.FindGameObjectWithTag("Player");

//         if (kid != null)
//         {
//             Rigidbody2D rb = kid.GetComponent<Rigidbody2D>();
//             if (rb != null)
//                 rb.simulated = enable;

//             KidMachine km = kid.GetComponent<KidMachine>();
//             if (km != null)
//                 km.enabled = enable;
//         }

//         if (monster != null)
//         {
//             Rigidbody2D rb = monster.GetComponent<Rigidbody2D>();
//             if (rb != null)
//                 rb.simulated = enable;

//             MonsterMove mm = monster.GetComponent<MonsterMove>();
//             if (mm != null)
//                 mm.enabled = enable;
//         }
//     }
// }

using System.Collections.Generic;
using UnityEngine;

public class CameraWaypointTour : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    public List<Transform> waypoints = new List<Transform>(); // ← Ahora es pública
    private int currentWaypointIndex = 0;
    private bool isTourActive = false;

    private void Update()
    {
        if (!isTourActive || waypoints.Count == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.05f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Count)
            {
                isTourActive = false;
            }
        }
    }

    public void StartTour()
    {
        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogWarning("StartTour: Lista de waypoints vacía.");
            return;
        }

        currentWaypointIndex = 0;
        isTourActive = true;

        transform.position = waypoints[0].position;
    }

    public void ClearAndSetNewWaypoints(List<Transform> newWaypoints)
    {
        waypoints.Clear();
        waypoints.AddRange(newWaypoints);
        StartTour();
    }
}
