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
    private List<GameObject> otherObjects = new List<GameObject>();
    private Animation anim;
    private int openDoorCounter = 0, enableObjectCouneter = 0, closeDoor = 0, disableObjectCouneter = 0;
    private bool allDoorsClosed = true;

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

        openDoorCounter = 0;
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

    public void OnDoorOpened()
    {
        if (doorsList.Count > 0 && doorsList.Count > openDoorCounter)
        {
            doorsList[openDoorCounter].OnDoorChangedAnim(true);
            doorsList[openDoorCounter].SetKeepActivated(true);
            openDoorCounter++;
        }
    }

    public void OnDoorClosed()
    {
        if (!allDoorsClosed && doorsList.Count > 0 && doorsList.Count > closeDoor)
            foreach (var door in doorsList)
            {
                doorsList[closeDoor].OnDoorChangedAnim(false);
                doorsList[closeDoor].SetKeepActivated(false);
                closeDoor++;

                if (doorsList.Count >= closeDoor)
                    allDoorsClosed = false;
            }
        else if (doorsList.Count > 0)
        {
            doorsList[closeDoor].OnDoorChangedAnim(false);
            doorsList[closeDoor].SetKeepActivated(false);
            closeDoor++;
        }
    }

    public void EnableObject()
    {
        if (otherObjects.Count > 0 && otherObjects.Count > enableObjectCouneter)
        {
            otherObjects[enableObjectCouneter].SetActive(true);
            otherObjects[enableObjectCouneter].SetActive(true);
            enableObjectCouneter++;
        }
    }

    public void DisableObject()
    {
        if (otherObjects.Count > 0 && otherObjects.Count > disableObjectCouneter)
        {
            otherObjects[disableObjectCouneter].SetActive(false);
            otherObjects[disableObjectCouneter].SetActive(false);
            disableObjectCouneter++;
        }
    }

    public void OnListUpdated(List<DoorBase> newDoorList, List<GameObject> otherObj)
    {
        doorsList = newDoorList;
        otherObjects = otherObj;
        openDoorCounter = 0; 
        closeDoor = 0;
        enableObjectCouneter = 0;
        disableObjectCouneter = 0;
        allDoorsClosed = false;
    }
}