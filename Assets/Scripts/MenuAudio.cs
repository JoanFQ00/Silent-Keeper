using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    private AudioSource music;

    public AudioClip ClickAudio;
    public AudioClip switchAudio;
    public AudioClip ambientAudio;

    void Start()
    {
        music = GetComponent<AudioSource>();

        if (ambientAudio != null)
        {
            music.clip = ambientAudio;
            music.loop = true;
            music.Play();
        }
    }

    public void OnClickAudio()
    {
        music.PlayOneShot(ClickAudio);
    }

    public void OnSwitchAudio()
    {
        music.PlayOneShot(switchAudio);
    }
}