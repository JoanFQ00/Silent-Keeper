using System.Collections.Generic;
using UnityEngine;

public class IntroRoomsManager : MonoBehaviour
{
    [SerializeField] private AnimationClip[] animationNames;
    [SerializeField] private GameObject cam, canvas;

    private AnimationClip animationToPlay;
    private Dictionary<AnimationClip, bool> roomsAnimations = new Dictionary<AnimationClip, bool>();
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
            roomsAnimations[clip] = false;

        roomInAnimation = false;

        canvas.SetActive(false);
        cam.SetActive(false);
    }

    public void SetAnimToPlay(AnimationClip roomClip)
    {
        animationToPlay = roomClip;
    }

    public void StartRoomAnim()
    {
            bool hasBeenAnimated = false;
        if (roomsAnimations.TryGetValue(animationToPlay, out bool isActive))
            hasBeenAnimated = isActive;

        if (anim == null || animationNames.Length == 0 || hasBeenAnimated || animationToPlay == null)
            return;

        roomsAnimations[animationToPlay] = true;
        roomInAnimation = true;

        canvas.SetActive(true);
        cam.SetActive(true);
        anim.Play(animationToPlay.name);
    }


    //AL ACABAR ANIMACIÓN, QUE LLAME A ESTA NUEVA FUNCIÓN (LA USAREMOS PARA CONGELAR EL MOVIMIENTO DE LOS PLAYERS, ENEMIGOS, HABILIDADES, INTERACCIÓN, TIEMPO)
    public void OnAnimationFinished()
    {
        roomInAnimation = false;
        canvas.SetActive(false);
        cam.SetActive(false);
    }
}