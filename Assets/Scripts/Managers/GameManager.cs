using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject monster, kid;
    [SerializeField] GameObject finalPanel, pausePanel;

    [SerializeField] private GameObject miniMapUI, monsterObjectTexture, kidObjectTexture;
    [SerializeField] private RenderTexture monsterTexture, kidTexture;

    [SerializeField] float remainingTime = 120f;
    [SerializeField] private GameObject[] mapSpawns = new GameObject[3];

    public bool isPaused;

    [HideInInspector]
    public float timerSpeedMultiplier = 1f;

    private float initialTime;
    private bool isCounting = true, stopCounting = false;

    public static Zone currentZone = Zone.ZONE_1;
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
        miniMapUI.SetActive(false);
        isPaused = false;

        monsterObjectTexture.GetComponent<Camera>().enabled = false;
        kidObjectTexture.GetComponent<Camera>().enabled = false;

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

        if (!CameraFollow.Instance.CheckPlayersInCam() && !TransitionCamAnimation.Instance.playersTransition)
        {
            miniMapUI.SetActive(true);

            if (CameraFollow.Instance.GetCameraTarget().gameObject == monster)
            {
                monsterObjectTexture.GetComponent<Camera>().enabled = false;
                kidObjectTexture.GetComponent<Camera>().enabled = true;
                miniMapUI.GetComponentInChildren<RawImage>().texture = kidTexture;
            }
            else if (CameraFollow.Instance.GetCameraTarget().gameObject == kid)
            {
                kidObjectTexture.GetComponent<Camera>().enabled = false;
                monsterObjectTexture.GetComponent<Camera>().enabled = true;
                miniMapUI.GetComponentInChildren<RawImage>().texture = monsterTexture;
            }
        }
        else
        {
            miniMapUI.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CameraFollow.Instance != null)
            {
                if (CameraFollow.Instance.GetCameraTarget().gameObject == monster)
                    CameraFollow.Instance.ChangeTarget(kid.transform);

                else if (CameraFollow.Instance.GetCameraTarget().gameObject == kid)
                    CameraFollow.Instance.ChangeTarget(monster.transform);
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

    //SPAWNEAR AL CAMBIAR DE ZONA AL ENTRAR A LA M QUINA EXPRENDEDORA
    public void SpawnPlayers()
    {
        switch (currentZone)
        {
            case Zone.ZONE_1:
                monster.transform.position = mapSpawns[0].transform.position;
                kid.transform.position = mapSpawns[0].transform.position;
                break;

            case Zone.ZONE_2:
                monster.transform.position = mapSpawns[0].transform.position;
                kid.transform.position = mapSpawns[0].transform.position;
                break;

            case Zone.ZONE_3:
                monster.transform.position = mapSpawns[0].transform.position;
                kid.transform.position = mapSpawns[0].transform.position;
                break;

            default:
                break;
        }
    }

    //CAMBIAR DE ZONA AL ENTRAR A LA M QUINA EXPRENDEDORA
    public void ChageZoneState(Zone newZone)
    {
        currentZone = newZone;
    }

    // til para m quina exprendedora
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