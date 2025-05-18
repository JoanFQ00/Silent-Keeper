using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    private float time;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isPaused)
            return;

    }
}