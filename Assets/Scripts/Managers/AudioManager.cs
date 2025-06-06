using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private Slider musicSlider;
    private Slider audioSlider;

    private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();

    public float sfxDelay = 2f;
    private float gameStartTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (PlayerPrefs.HasKey("MusicVolume"))
                musicVolume = PlayerPrefs.GetFloat("MusicVolume");

            if (PlayerPrefs.HasKey("SFXVolume"))
                sfxVolume = PlayerPrefs.GetFloat("SFXVolume");

            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.loop = true;
            }

            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
            }

            musicSource.volume = musicVolume;
            sfxSource.volume = sfxVolume;

            LoadSFX();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameStartTime = Time.time;
    }

    void OnEnable()
    {
        DynamicButt.PassMusicSlider += GetSlider;
        DynamicButt.PassAudioSlider += GetSlider;
    }

    void OnDisable()
    {
        DynamicButt.PassMusicSlider -= GetSlider;
        DynamicButt.PassAudioSlider -= GetSlider;
    }

    void GetSlider(Slider slider)
    {
        if (slider.gameObject.name == "VolumeM_Sl")
        {
            musicSlider = slider;
            musicSlider.value = musicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        else if (slider.gameObject.name == "VolumeS_Sl")
        {
            audioSlider = slider;
            audioSlider.value = sfxVolume;
            audioSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    void SetMusicVolume(float value)
    {
        musicVolume = value;
        musicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    void SetSFXVolume(float value)
    {
        sfxVolume = value;
        sfxSource.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    private void LoadSFX()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("SFX");

        foreach (AudioClip clip in clips)
        {
            sfxClips[clip.name] = clip;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameStartTime = Time.time; // Resetear el delay al cargar una nueva escena

        string sceneName = scene.name;

        if (scene.buildIndex == 0 || scene.buildIndex == 1 || scene.buildIndex == 5)
            musicSource.Stop();
        else
        {
            AudioClip musicClip = Resources.Load<AudioClip>("Audio/" + sceneName);

            if (musicClip != null)
            {
                musicSource.clip = musicClip;
                musicSource.Play();
            }
        }
    }

    public void PlaySFX(string name)
    {
        if (Time.time - gameStartTime < sfxDelay)
            return;

        if (sfxClips.ContainsKey(name))
        {
            sfxSource.PlayOneShot(sfxClips[name]);
        }
    }
}
