using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroRoomsManager : MonoBehaviour
{
    [SerializeField] private AnimationClip[] animationNames;
    [SerializeField] private GameObject cam, canvas, camTransitionParticles;
    [SerializeField] private float speedAnimSkip = 10.0f;

    private AnimationClip animationToPlay;
    private Dictionary<string, bool> roomsAnimations = new Dictionary<string, bool>();
    private List<DoorBase> doorsList = new List<DoorBase>();
    private List<GameObject> otherObjects = new List<GameObject>();
    private Animation anim;
    private int doorCounter = 0, objectCouneter = 0;

    [HideInInspector]
    public bool roomInAnimation;

    public static Action OnAnimationStarted, OnAnimationDisable, OnCircusEnemyAnim;

    public static IntroRoomsManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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

        doorCounter = 0;
        camTransitionParticles.SetActive(true);
        canvas.SetActive(false);
        cam.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && roomInAnimation)
            SkipAnimationAndRunEvents();
    }

    private void SkipAnimationAndRunEvents()
    {
        if (anim == null || animationToPlay == null)
            return;

        StartCoroutine(FastForwardAnimation());
    }

    private IEnumerator FastForwardAnimation()
    {
        anim[animationToPlay.name].speed = speedAnimSkip;
        anim.Play(animationToPlay.name);

        while (anim.isPlaying)
            yield return null;

        anim[animationToPlay.name].speed = 1f;

        OnAnimationFinished();
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
        if (doorsList.Count > 0 && doorsList.Count > doorCounter)
        {
            doorsList[doorCounter].OnDoorChangedAnim(true);
            doorsList[doorCounter].SetKeepActivated(true);
            doorCounter++;
        }
    }

    public void OnDoorClosed()
    {
        if (doorsList.Count > 0 && doorsList.Count > doorCounter)
        {
            doorsList[doorCounter].OnDoorChangedAnim(false);
            doorsList[doorCounter].SetKeepActivated(false);
            doorCounter++;
        }
    }

    public void EnableObject()
    {
        if (otherObjects.Count > 0 && otherObjects.Count > objectCouneter)
        {
            otherObjects[objectCouneter].SetActive(true);
            objectCouneter++;
        }
    }

    public void DisableObject()
    {
        if (otherObjects.Count > 0 && otherObjects.Count > objectCouneter)
        {
            otherObjects[objectCouneter].SetActive(false);
            objectCouneter++;
        }
    }

    public void OnListUpdated(List<DoorBase> newDoorList, List<GameObject> otherObj)
    {
        doorsList = newDoorList;
        otherObjects = otherObj;
        doorCounter = 0;
        objectCouneter = 0;
    }

    public void OnCircusEnemyActivated()
    {
        OnCircusEnemyAnim?.Invoke();
    }
}