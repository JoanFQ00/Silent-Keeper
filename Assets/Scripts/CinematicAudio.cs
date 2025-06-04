using UnityEngine;
using System.Collections.Generic;

public class CinematicAudio : MonoBehaviour
{
    public List<AudioClip> sonidos;

    private AudioSource audioSource;
    private int counterClip;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        counterClip = 0;
    }

    public void PlayCinematicSound()
    {
        if (audioSource != null && sonidos != null)
        {
            audioSource.PlayOneShot(sonidos[counterClip]);
            counterClip++;
        }
    }

    public void StartGame()
    {
        SceneChanger.instance.NextZone();
    }

    public void OnCinematicFinished()
    {
        GameManager.Instance.StopCounting();
        SceneChanger.instance.LoadMenu();
    }
}