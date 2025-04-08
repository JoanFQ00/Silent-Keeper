using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject finalPanel, _player;

    public static GameManager Instance;

    public static Action<string> OnTimerUpdating;

    [SerializeField] float tiempoRestante = 120f;
    public float timerSpeedMultiplier = 1f;

    private bool estaContando = true;

    private void Start()
    {
        finalPanel.SetActive(false);
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
                RestartZone();
            }

            ActualizarTexto();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            RestartZone();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Saliendo de la aplicación...");
        }
    }

    void ActualizarTexto()
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60f);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60f);
        OnTimerUpdating?.Invoke($"{minutos:00}:{segundos:00}");
    }

    public void RestartZone()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Útil para máquina exprendedora
    public void ActiveFinalPanel()
    {
        _player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        finalPanel.SetActive(true);
    }
}