using System;
using UnityEditor.Overlays;
using UnityEngine;

public class TransitionCamAnimation : MonoBehaviour
{
    public static TransitionCamAnimation Instance;

    [HideInInspector] 
    public bool playersInTransition, canRunLastAnimation;

    [SerializeField] private AnimationClip[] animationNames;
    
    private Camera doorCamera;
    private int currentIndex = 0;

    private Animation anim;
    private int doorKeyTP;
    private Transform doorPosition;
    private GameObject mainCamera;

    public static Action<int> OnChangedRoom;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        doorCamera = GetComponentInChildren<Camera>();
        anim = GetComponent<Animation>();

        doorKeyTP = 0;

        doorCamera.enabled = false;
        playersInTransition = false;
        canRunLastAnimation = false;
    }

    private void Update()
    {
        if (mainCamera.GetComponent<Camera>().enabled == true)
            OnPlayersChanged(mainCamera.transform);

        if (Input.GetKeyUp(KeyCode.F) && canRunLastAnimation)
        {
            ChangeToNextTransition();
            canRunLastAnimation = false;
        }
    }

    private void OnEnable()
    {
        DoorBase.OnStartCamTransition += ExecuteFirstTransition;
    }

    private void OnDisable()
    {
        DoorBase.OnStartCamTransition -= ExecuteFirstTransition;
    }

    private void ExecuteFirstTransition(int doorKey, Transform doorTransform)
    {
        if (anim == null || animationNames.Length == 0) return;

        string firstAnim = animationNames[0].name;

        if (anim.GetClip(firstAnim) != null)
        {
            playersInTransition = true;
            doorCamera.enabled = true;
            mainCamera.GetComponent<Camera>().enabled = false;
            anim.Play(firstAnim);
            doorKeyTP = doorKey;
            doorPosition = doorTransform;

            currentIndex += 1;
        }
    }

    public void ChangePlayerPosition() //Mitad 3a
    {
        OnChangedRoom?.Invoke(doorKeyTP);
    }

    public void OnPlayersChanged(Transform monsterTransform)
    {
        float zAxis = doorCamera.transform.position.z;
        doorCamera.transform.position = new Vector3(monsterTransform.position.x, monsterTransform.position.y, zAxis);
    }

    public void ChangeToNextTransition() //Final 1a
    {
        if (anim == null || currentIndex >= animationNames.Length) return;

        string nextAnim = animationNames[currentIndex].name;

        if (anim.GetClip(nextAnim) != null)
        {
            float zAxis = doorCamera.transform.position.z;
            doorCamera.transform.position = new Vector3(doorPosition.position.x, doorPosition.position.y, zAxis);
            anim.Play(nextAnim);
            currentIndex++;
        }
    }

    public void CanRunLastTransition() //Final 2a
    {
        canRunLastAnimation = true;
    }

    public void AnimationFinished() //Final 3a
    {
        ResetAnimationIndex();
        playersInTransition = false;
        doorCamera.enabled = false;
        mainCamera.GetComponent<Camera>().enabled = true;
    }

    private void ResetAnimationIndex()
    {
        currentIndex = 0;
    }

    //MIRAR POSICIÓN HACIA DONDE SE EJECUTA LA ANIMACIÓN (PUERTA)
}