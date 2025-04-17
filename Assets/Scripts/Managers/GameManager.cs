using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject monster, kid;
    [SerializeField] GameObject finalPanel, pausePanel;
    [SerializeField] float tiempoRestante = 120f;

    public float timerSpeedMultiplier = 1f;
    public bool isPaused;

    private bool estaContando = true;
    private GameObject mainCamera;

    public static GameManager Instance;
    public static Action<string> OnTimerUpdating;

    private void Start()
    {
        finalPanel.SetActive(false);
        isPaused = false;
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
        if (estaContando)
        {
            tiempoRestante -= Time.deltaTime * timerSpeedMultiplier;

            if (tiempoRestante <= 0f)
            {
                tiempoRestante = 0f;
                estaContando = false;
                SceneChanger.instance.RestartZone();
            }

            ActualizarTexto();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (mainCamera != null)
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

    void ActualizarTexto()
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60f);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60f);
        OnTimerUpdating?.Invoke($"{minutos:00}:{segundos:00}");
    }

    //Útil para máquina exprendedora
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
}