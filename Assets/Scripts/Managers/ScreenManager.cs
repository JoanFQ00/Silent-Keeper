using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private Toggle toggle; // Referencia al toggle para pantalla completa
    public TMP_Dropdown dropdown; // Dropdown para seleccionar resolución

    private Resolution[] resolutions; // Todas las resoluciones disponibles
    private List<Resolution> filtredRes; // Resoluciones filtradas por frecuencia de refresco
    private List<string> options; // Opciones de texto para el dropdown

    private RefreshRate currentRefreshRate; // Frecuencia de refresco actual del monitor
    private int currentResolutionIdx; // Índice de la resolución actual dentro de las filtradas

    void Awake()
    {
        // Si no hay preferencia de pantalla completa guardada, se guarda por defecto en 1 (true)
        if (!PlayerPrefs.HasKey("Fullscreen"))
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
        }

        // Carga el estado del toggle desde PlayerPrefs
        Load();

        // Sincroniza el estado real de pantalla completa con el toggle
        toggle.isOn = Screen.fullScreen;

        // Inicializa y llena el dropdown con las resoluciones compatibles
        CheckResolution();

        // Revisa si es la primera vez que se ejecuta la app, o no hay resolución guardada
        if (!PlayerPrefs.HasKey("numRes"))
        {
            // Busca la resolución 1920x1400 dentro de las filtradas
            int index1920x1400 = filtredRes.FindIndex(res => res.width == 1920 && res.height == 1400);

            // Si la encontró, la aplica y guarda
            if (index1920x1400 != -1)
            {
                ChangeRes(index1920x1400);
                dropdown.value = index1920x1400;
                PlayerPrefs.SetInt("numRes", index1920x1400);
                PlayerPrefs.Save();
            }
            else
            {
                Debug.LogWarning("No se encontró la resolución 1920x1400 en la lista filtrada.");
            }
        }
    }


    public void ChangeScreen(bool fullScreen)
    {
        // Cambia el modo de pantalla completa y guarda la preferencia
        Screen.fullScreen = fullScreen;
        toggle.isOn = fullScreen;
        Save();
    }

    private void Load()
    {
        // Carga el estado de pantalla completa desde PlayerPrefs
        toggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1;
    }

    private void Save()
    {
        // Guarda el estado del toggle en PlayerPrefs y lo escribe en disco
        PlayerPrefs.SetInt("Fullscreen", toggle.isOn ? 1 : 0);
        PlayerPrefs.Save(); // Asegura que se guarde inmediatamente
    }

    public void CheckResolution()
    {
        resolutions = Screen.resolutions; // Obtiene todas las resoluciones disponibles
        filtredRes = new List<Resolution>(); // Inicializa la lista filtrada
        dropdown.ClearOptions(); // Limpia el dropdown antes de llenarlo

        currentRefreshRate = Screen.currentResolution.refreshRateRatio; // Obtiene la frecuencia actual del monitor

        // Filtra las resoluciones para que solo se incluyan las que coincidan con la frecuencia actual
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.numerator == currentRefreshRate.numerator &&
                resolutions[i].refreshRateRatio.denominator == currentRefreshRate.denominator)
            {
                filtredRes.Add(resolutions[i]);
            }
        }

        options = new List<string>(); // Lista de textos a mostrar en el dropdown

        for (int i = 0; i < filtredRes.Count; i++)
        {
            // Formatea la opción como "1920x1080 @ 60Hz"
            string option = $"{filtredRes[i].width}x{filtredRes[i].height} @ {filtredRes[i].refreshRateRatio.value:0}Hz";
            options.Add(option);

            // Guarda el índice de la resolución actual para seleccionarla por defecto
            if (filtredRes[i].width == Screen.width && filtredRes[i].height == Screen.height)
            {
                currentResolutionIdx = i;
            }
        }

        dropdown.AddOptions(options); // Agrega las opciones al dropdown

        // Usa el índice guardado por el usuario o el de la resolución actual
        int savedIdx = PlayerPrefs.GetInt("numRes", currentResolutionIdx);

        // Asegura que el índice esté dentro del rango válido
        dropdown.value = Mathf.Clamp(savedIdx, 0, filtredRes.Count - 1);
        dropdown.RefreshShownValue(); // Actualiza la UI del dropdown
    }

    public void ChangeRes(int resIndx)
    {
        // Guarda el índice seleccionado por el usuario
        PlayerPrefs.SetInt("numRes", resIndx);
        PlayerPrefs.Save(); // Guarda inmediatamente

        // Cambia la resolución si el índice es válido
        if (resIndx >= 0 && resIndx < filtredRes.Count)
        {
            Resolution resolution = filtredRes[resIndx];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }
}
