using UnityEngine;
using TMPro;

public class TimerTxt : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.OnTimerUpdating += UploadTimerText;
    }

    private void OnDisable()
    {
        GameManager.OnTimerUpdating -= UploadTimerText;
    }

    private void UploadTimerText(string timer)
    {
        GetComponent<TextMeshProUGUI>().text = "" + timer;
    }
}