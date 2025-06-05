using UnityEngine;
using System.Collections.Generic;

public class CameraWaypointTour : MonoBehaviour
{
    public float moveSpeed = 3f;
    public List<Transform> waypoints;
    private int currentIndex = 0;
    private bool isTourActive = false;
    private CameraFollow cameraFollow;

    void Start()
    {
        cameraFollow = GetComponent<CameraFollow>();
        if (waypoints != null && waypoints.Count > 0)
        {
            StartTour(waypoints);
        }
    }

    void Update()
    {
        if (isTourActive && waypoints != null && currentIndex < waypoints.Count)
        {
        }
    }

    public void StartTour(List<Transform> tourPoints)
    {
        if (tourPoints == null || tourPoints.Count == 0)
        {
            return;
        }

        waypoints = tourPoints;
        currentIndex = 0;
        isTourActive = true;

        GameManager.Instance.isPaused = true;
        GameManager.Instance.StopCounting();

        ReturnCharacters(true);

        cameraFollow.enabled = false;
    }

    void EndTour()
    {
        isTourActive = false;

        cameraFollow.enabled = true;

        GameManager.Instance.isPaused = false;
        GameManager.Instance.RestartTime();

        ReturnCharacters(true);
    }

    void ReturnCharacters(bool enable)
    {
        GameObject kid = GameObject.FindGameObjectWithTag("Kid");
        GameObject monster = GameObject.FindGameObjectWithTag("Player");

        if (kid != null)
        {
            Rigidbody2D rb = kid.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.simulated = enable;

            KidMachine km = kid.GetComponent<KidMachine>();
            if (km != null)
                km.enabled = enable;
        }

        if (monster != null)
        {
            Rigidbody2D rb = monster.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.simulated = enable;

            MonsterMove mm = monster.GetComponent<MonsterMove>();
            if (mm != null)
                mm.enabled = enable;
        }
    }
}