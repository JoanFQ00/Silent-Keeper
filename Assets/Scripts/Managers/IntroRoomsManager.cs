using System.Collections.Generic;
using UnityEngine;

public class IntroRoomsManager : MonoBehaviour
{
    [SerializeField] private AnimationClip[] animationNames;
    [SerializeField] private GameObject cam, canvas, camTransitionParticles;

    private AnimationClip animationToPlay;
    private Dictionary<string, bool> roomsAnimations = new Dictionary<string, bool>();
    private Animation anim;

    [HideInInspector]
    public bool roomInAnimation;

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

        foreach (AnimationClip clip in animationNames)
        {
            if (clip != null && !roomsAnimations.ContainsKey(clip.name))
                roomsAnimations[clip.name] = false;
        }

        roomInAnimation = false;

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
    }
}