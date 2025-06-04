using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public float musicVolume = 0.5f;
    public float sfxVolume = 1f;

    private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

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
        string sceneName = scene.name;

        if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 0)
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
        if (sfxClips.ContainsKey(name))
        {
            sfxSource.PlayOneShot(sfxClips[name]);
        }
        else
        {
            Debug.LogWarning($"SFX clip '{name}' not found in loaded clips.");
        }
    }
}
