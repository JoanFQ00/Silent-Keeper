using UnityEngine;

public class SkipUI : MonoBehaviour
{
    public GameObject skipText;

    private void Start()
    {
        skipText.SetActive(false);
    }

    private void OnEnable()
    {
        IntroRoomsManager.OnAnimationStarted += Activate;
        IntroRoomsManager.OnAnimationDisable += Disable;
    }

    private void OnDisable()
    {
        IntroRoomsManager.OnAnimationStarted -= Activate;
        IntroRoomsManager.OnAnimationDisable -= Disable;
    }

    private void Activate()
    {
        skipText.SetActive(true);
    }

    private void Disable()
    {
        skipText.SetActive(false);
    }
}