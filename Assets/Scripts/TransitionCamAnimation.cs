using System;
using UnityEngine;

public class TransitionCamAnimation : MonoBehaviour
{
    public static TransitionCamAnimation Instance;

    [HideInInspector]
    public bool playersTransition;

    private Animation anim;
    private int doorKeyTP;

    public static Action<int> OnChangedRoom;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        anim = GetComponent<Animation>();
        doorKeyTP = 0;
        playersTransition = false;
    }

    private void OnEnable()
    {
        DoorBase.OnStartCamTransition += ExecuteTransition;
    }

    private void OnDisable()
    {
        DoorBase.OnStartCamTransition -= ExecuteTransition;
    }

    private void ExecuteTransition(int doorKey)
    {
        if (anim != null && anim.clip != null)
        {
            anim.Play();
            playersTransition = true;
            doorKeyTP = doorKey;
        }
    }

    public void ChangePlayerPosition()
    {
        OnChangedRoom?.Invoke(doorKeyTP);
    }

    public void AnimationFinished()
    {
        playersTransition = false;
    }

    //HACER FUNCIÓN ANIM CAMARAS
}