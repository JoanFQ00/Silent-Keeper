using UnityEngine;

public class VendingMachineUI : MonoBehaviour
{
    public void OnEndInitialCinematic()
    {
        VendingMachineBase.Instance.OnEndCinematic();
    }

    public void OnEndFinalCinematic()
    {
        VendingMachineBase.Instance.ReanudeToGame();
    }
}