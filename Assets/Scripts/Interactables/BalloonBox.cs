using UnityEngine;
using System;

public class BalloonBox : MonoBehaviour, IInteractableWithButton
{
    public LayerMask interactableLayer;

    private static uint boxBaloonAmount, maxAmount;

    public static uint balloonsOnInventroy;

    public static Action OnBallonUsed;

    private void Start()
    {
        balloonsOnInventroy = 0;
        maxAmount = 3;
        boxBaloonAmount = maxAmount;
    }

    public static void AddBallon()
    {
        if (boxBaloonAmount < maxAmount)
            boxBaloonAmount += 1;
    }

    public void Interacted() 
    {
        if (boxBaloonAmount > 0)
        {
            balloonsOnInventroy += 1;
            boxBaloonAmount -= 1;
        }
    }

    public void Interacted(GameObject other, GameObject thisObject) { }

    //QUITAR IMAGEN DEL HUD SI HAY 0
}