using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class IntroRoomsManager : MonoBehaviour
{
    [SerializeField] private AnimationClip[] animationNames;
    [SerializeField] private GameObject cam, canvas, camTransitionParticles;

    private AnimationClip animationToPlay;
    private Dictionary<string, bool> roomsAnimations = new Dictionary<string, bool>();
    private List<DoorBase> doorsList = new List<DoorBase>();
    private List<DoorBase> wallList = new List<DoorBase>();
    private Animation anim;
    private int openCounter = 0, closeDoor = 0;

    [HideInInspector]
    public bool roomInAnimation;

    public static Action OnAnimationStarted, OnAnimationDisable;

    public static IntroRoomsManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && roomInAnimation)
            OnAnimationFinished();
    }

    private void Start()
    {
        anim = GetComponent<Animation>();
        anim.enabled = true;

        foreach (AnimationClip clip in animationNames)
        {
            if (clip != null && !roomsAnimations.ContainsKey(clip.name))
                roomsAnimations[clip.name] = false;
        }

        roomInAnimation = false;

        openCounter = 0;
        closeDoor = 0;

        camTransitionParticles.SetActive(true);
        canvas.SetActive(false);
        cam.SetActive(false);
    }

    public void SetAnimToPlay(AnimationClip roomClip)
    {
        if (roomClip == null)
            return;

        animationToPlay = roomClip;
    }

    public void StartRoomAnim()
    {
        if (animationToPlay == null || anim == null || animationNames.Length == 0)
        {
            Debug.LogWarning("StartRoomAnim cancelado por valores nulos.");
            return;
        }

        bool hasBeenAnimated = roomsAnimations.TryGetValue(animationToPlay.name, out bool isActive) && isActive;
        if (hasBeenAnimated)
            return;

        OnAnimationStarted?.Invoke();
        anim.enabled = true;

        roomsAnimations[animationToPlay.name] = true;
        roomInAnimation = true;

        camTransitionParticles.SetActive(false);
        canvas.SetActive(true);
        cam.SetActive(true);

        anim.Play(animationToPlay.name);
    }

    public void OnAnimationFinished()
    {
        roomInAnimation = false;
        camTransitionParticles.SetActive(true);
        canvas.SetActive(false);
        cam.SetActive(false);

        anim.enabled = false;
        OnAnimationDisable?.Invoke();
    }

    //PONER WALLS TMB
    public void OnDoorOpened()
    {
        Debug.Log(doorsList[closeDoor]);

        if (doorsList.Count > 0)
            doorsList[closeDoor].OnDoorChangedAnim(true);

        if (wallList.Count > 0)
            wallList[closeDoor].OnDoorChangedAnim(true);

        if (doorsList.Count > 0 || wallList.Count > 0)
            openCounter++;
    }

    public void OnDoorClosed()
    {
        if (doorsList.Count > 0)
            doorsList[closeDoor].OnDoorChangedAnim(false);

        if (wallList.Count > 0)
            wallList[closeDoor].OnDoorChangedAnim(false);

        if (doorsList.Count > 0 || wallList.Count > 0)
            closeDoor++;
    }

    public void OnListUpdated(List<DoorBase> newDoorList, List<DoorBase> newWallList)
    {
        doorsList = newDoorList;
        wallList = newWallList;

        openCounter = 0; 
        closeDoor = 0;
    }
}