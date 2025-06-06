using System;
using UnityEngine;
using UnityEngine.UI;
using static HeadHUD;

public class GameManager : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] GameObject monster, kid;
    [SerializeField] GameObject finalPanel, pausePanel;

    [Header("Minimap & Cameras")]
    [SerializeField] private GameObject miniMapUI, monsterObjectTexture, kidObjectTexture;
    [SerializeField] private RenderTexture monsterTexture, kidTexture;

    [Header("Timer")]
    [SerializeField] float remainingTime = 120f;
    [SerializeField] private GameObject mapSpawn;

    public bool isPaused;

    [HideInInspector] public float timerSpeedMultiplier = 1f;

    private float initialTime;

    public static bool lastCheckpoint = false;
    public static Zone currentZone = Zone.ZONE_1;
    public static GameManager Instance;
    public static Action<string> OnTimerUpdating;
    public static Vector3 currentSpawn;

    public enum Zone
    {
        ZONE_1 = 0,
        ZONE_2,
        ZONE_3
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

    private void Start()
    {
        Time.timeScale = 1f;

        finalPanel.SetActive(false);
        miniMapUI.SetActive(false);
        isPaused = false;

        monsterObjectTexture.GetComponent<Camera>().enabled = false;
        kidObjectTexture.GetComponent<Camera>().enabled = false;


        initialTime = remainingTime;

        if (!lastCheckpoint)
            currentSpawn = mapSpawn.transform.position;

        SpawnInitialPlayers();
    }

    private void Update()
    {
        if (!CameraFollow.Instance.CheckPlayersInCam() && !TransitionCamAnimation.Instance.playersInTransition)
        {
            if (CameraFollow.Instance.GetCameraTarget().gameObject == monster && kid.GetComponent<KidMachine>().GetCurrentState() != KidMachine.States.HiddenInLocker)
            {
                miniMapUI.SetActive(true);

                monsterObjectTexture.GetComponent<Camera>().enabled = false;
                kidObjectTexture.GetComponent<Camera>().enabled = true;
                miniMapUI.GetComponentInChildren<RawImage>().texture = kidTexture;
            }
            else if (CameraFollow.Instance.GetCameraTarget().gameObject == kid && monster.GetComponent<MonsterMove>().GetMonsterState() != KidMachine.States.HiddenInLocker)
            {
                miniMapUI.SetActive(true);

                kidObjectTexture.GetComponent<Camera>().enabled = false;
                monsterObjectTexture.GetComponent<Camera>().enabled = true;
                miniMapUI.GetComponentInChildren<RawImage>().texture = monsterTexture;
            }
            else if (TransitionCamAnimation.Instance.playersInTransition)
            {
                miniMapUI.SetActive(true);

                kidObjectTexture.GetComponent<Camera>().enabled = false;
                monsterObjectTexture.GetComponent<Camera>().enabled = true;
                miniMapUI.GetComponentInChildren<RawImage>().texture = monsterTexture;
            }
            else if (kid.GetComponent<KidMachine>().GetCurrentState() != KidMachine.States.HiddenInLocker || monster.GetComponent<MonsterMove>().GetMonsterState() != KidMachine.States.HiddenInLocker)
                miniMapUI.SetActive(false);
        }
        else if (CameraFollow.Instance.GetCameraReturned())
        {
            miniMapUI.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.R) && !TransitionCamAnimation.Instance.playersInTransition)
        {
            if (CameraFollow.Instance != null)
            {
                if (CameraFollow.Instance.GetCameraTarget().gameObject == monster)
                {
                    CameraFollow.Instance.ChangeTarget(kid.transform);
                    HeadHUD.Instance.OnFocusChanged(FocusTarget.Player2);
                }
                else if (CameraFollow.Instance.GetCameraTarget().gameObject == kid)
                {
                    CameraFollow.Instance.ChangeTarget(monster.transform);
                    HeadHUD.Instance.OnFocusChanged(FocusTarget.Player1);
                }
            }
        }
    }

    private void SpawnInitialPlayers()
    {
        Vector3 spawnPoint = currentSpawn != null ? currentSpawn : mapSpawn.transform.position;
        float offset = 1.5f;

        monster.transform.position = spawnPoint + Vector3.left * offset;
        kid.transform.position = spawnPoint + Vector3.right * offset;
    }

    public void SpawnPlayers()
    {
        Vector3 spawnPoint = currentSpawn != null ? currentSpawn : mapSpawn.transform.position;
        float offset = 1.5f;

        if (currentSpawn == mapSpawn.transform.position)
            SceneChanger.instance.RestartZone();
        else
        {
            monster.transform.position = spawnPoint + Vector3.left * offset;
            kid.transform.position = spawnPoint + Vector3.right * offset;
        }
    }


    public void ChageZoneState(Zone newZone)
    {
        currentZone = newZone;
    }

    public void ActiveFinalPanel()
    {
        monster.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        finalPanel.SetActive(true);
    }

    public void Pause()
    {
        bool isNowPaused = !pausePanel.activeSelf;
        pausePanel.SetActive(isNowPaused);
        isPaused = isNowPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }


    public void StopCounting()
    {
    }

    public void RestartTime()
    {
        remainingTime = initialTime;
    }
}