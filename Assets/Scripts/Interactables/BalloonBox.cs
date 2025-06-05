using System;
using UnityEngine;

public class BalloonBox : MonoBehaviour, IInteractableWithButton
{
    public LayerMask interactableLayer;
    private static uint boxBalloonAmount;
    private const uint maxBalloonPerBox = 3;
    private const uint maxBalloonsInInventory = 3;

    public static uint balloonsInInventory = 0;

    public static Action OnBalloonUsed;

    private bool initialized = false;

    private void Start()
    {
        if (!initialized)
        {
            boxBalloonAmount = maxBalloonPerBox;
            initialized = true;
        }
    }

    public static void AddBalloon()
    {
        if (boxBalloonAmount < maxBalloonPerBox)
            boxBalloonAmount += 1;
    }

    public void Interacted()
    {
        if (boxBalloonAmount > 0 && balloonsInInventory < maxBalloonsInInventory)
        {
            balloonsInInventory += 1;
            boxBalloonAmount -= 1;
            SkillManager.Instance.OnBalloonTaked();
        }
    }

    public void Interacted(GameObject other, GameObject thisObject) { }
}