using System;
using UnityEngine;

public class TransitionCamAnimation : MonoBehaviour
{
    public static TransitionCamAnimation Instance;

    [HideInInspector]
    public bool playersInTransition, canRunLastAnimation;

    [SerializeField] private AnimationClip[] animationNames;

    private int currentIndex = 0;

    private Animation anim;
    private int doorKeyTP;
    private Transform doorPosition;
    private GameObject mainCamera;

    private bool controlAlphaOffset = false;

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
        anim = GetComponent<Animation>();

        doorKeyTP = 0;
        playersInTransition = false;
        canRunLastAnimation = false;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && canRunLastAnimation)
        {
            ChangeToNextTransition();
            canRunLastAnimation = false;
        }

        if (controlAlphaOffset && AlphaThresholdController.Instance != null)
        {
            RectTransform rt = AlphaThresholdController.Instance.GetComponent<RectTransform>();

            Vector2 localMousePos;
            bool inside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rt,
                Input.mousePosition,
                Camera.main,
                out localMousePos
            );

            if (inside)
            {
                Vector2 size = rt.rect.size;

                float percentX = Mathf.Clamp01((localMousePos.x / size.x) + 0.5f);
                float percentY = Mathf.Clamp01((localMousePos.y / size.y) + 0.5f);

                float offsetRange = 0.25f;

                AlphaThresholdController.Instance.offsetX = Mathf.Lerp(-offsetRange, offsetRange, 1f - percentX);
                AlphaThresholdController.Instance.offsetY = Mathf.Lerp(-offsetRange, offsetRange, 1f - percentY);
            }
            else
            {
                AlphaThresholdController.Instance.offsetX = 0f;
                AlphaThresholdController.Instance.offsetY = 0f;
            }
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
            anim.Play(firstAnim);
            doorKeyTP = doorKey;
            doorPosition = doorTransform;

            currentIndex += 1;
        }
    }

    public void ChangePlayerPosition() // Mitad 3a
    {
        controlAlphaOffset = false;
        OnChangedRoom?.Invoke(doorKeyTP);
    }

    public void ChangeToNextTransition() // Final 1a
    {
        if (anim == null || currentIndex >= animationNames.Length) return;

        string nextAnim = animationNames[currentIndex].name;

        if (anim.GetClip(nextAnim) != null)
        {
            float zAxis = mainCamera.transform.position.z;
            mainCamera.transform.position = new Vector3(doorPosition.position.x, doorPosition.position.y, zAxis);
            anim.Play(nextAnim);
            currentIndex++;
        }
    }

    public void CanRunLastTransition() // Final 2a
    {
        canRunLastAnimation = true;
        controlAlphaOffset = true;
    }

    public void AnimationFinished() // Final 3a
    {
        ResetAnimationIndex();
        playersInTransition = false;
    }

    private void ResetAnimationIndex()
    {
        currentIndex = 0;
    }
}