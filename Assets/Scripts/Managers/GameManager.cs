using NUnit.Framework;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject monster, kid;
    [SerializeField] GameObject finalPanel, pausePanel;
    [SerializeField] float remainingTime = 120f;
    [SerializeField] private GameObject[] mapSpawns = new GameObject[3];

    public bool isPaused;

    [HideInInspector]
    public float timerSpeedMultiplier = 1f;

    private float initialTime;
    private bool isCounting = true, stopCounting = false;
    private GameObject mainCamera;
    private static Zone currentZone = Zone.ZONE_1;

    public static GameManager Instance;
    public static Action<string> OnTimerUpdating;

    public enum Zone
    {
        ZONE_1 = 0,
        ZONE_2,
        ZONE_3
    }

    private void Start()
    {
        finalPanel.SetActive(false);
        isPaused = false;

        SpawnPlayers();

        initialTime = remainingTime;
    }

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

    public void SetInitialCameraReference(GameObject camera)
    {
        mainCamera = camera;
        mainCamera.GetComponent<CameraFollow>().ChangeTarget(monster.transform);
    }

    void Update()
    {
        if (isCounting && !stopCounting)
        {
            remainingTime -= Time.deltaTime * timerSpeedMultiplier;

            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                isCounting = false;
                SceneChanger.instance.RestartZone();
            }

            RefreshText();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (mainCamera != null && !RoomBase.playersAreInRoom)
            {
                if (mainCamera.GetComponent<CameraFollow>().GetCameraTarget().gameObject == monster)
                    mainCamera.GetComponent<CameraFollow>().ChangeTarget(kid.transform);
                else
                    mainCamera.GetComponent<CameraFollow>().ChangeTarget(monster.transform);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneChanger.instance.RestartZone();
        }
    }

    private void RefreshText()
    {
        int minutos = Mathf.FloorToInt(remainingTime / 60f);
        int segundos = Mathf.FloorToInt(remainingTime % 60f);
        OnTimerUpdating?.Invoke($"{minutos:00}:{segundos:00}");
    }

    //SPAWNEAR AL CAMBIAR DE ZONA AL ENTRAR A LA M�QUINA EXPRENDEDORA
    public void SpawnPlayers()
    {
        switch (currentZone)
        {
            case Zone.ZONE_1:
                monster.transform.position = mapSpawns[0].transform.position;
                kid.transform.position = mapSpawns[0].transform.position;
                break;

            case Zone.ZONE_2:
                monster.transform.position = mapSpawns[1].transform.position;
                kid.transform.position = mapSpawns[1].transform.position;
                break;

            case Zone.ZONE_3:
                monster.transform.position = mapSpawns[2].transform.position;
                kid.transform.position = mapSpawns[2].transform.position;
                break;

            default:
                break;
        }
    }

    //CAMBIAR DE ZONA AL ENTRAR A LA M�QUINA EXPRENDEDORA
    public void ChageZoneState(Zone newZone)
    {
        currentZone = newZone;
    }

    //�til para m�quina exprendedora
    public void ActiveFinalPanel()
    {
        monster.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        finalPanel.SetActive(true);
    }

    public void Pause()
    {
        bool active = pausePanel.activeSelf;
        pausePanel.SetActive(!active);

        if (!active) { isPaused = true; }
        else { isPaused = false; }
    }

    public void StopCounting()
    {
        stopCounting = true;
    }

    public void RestartTime()
    {
        stopCounting = false;
        remainingTime = initialTime;
    }
}