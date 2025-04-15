using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject finalPanel, _player;
    [SerializeField] GameObject pausePanel;

    public static GameManager Instance;

    public static Action<string> OnTimerUpdating;
    [SerializeField] float tiempoRestante = 120f;
    public float timerSpeedMultiplier = 1f;
    private bool estaContando = true;
    public bool isPaused;

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
        _player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
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