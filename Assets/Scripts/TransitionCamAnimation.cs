using System;
using UnityEngine;
using UnityEngine.UI;

public class TransitionCamAnimation : MonoBehaviour
{
    public static TransitionCamAnimation Instance;

    [HideInInspector]
    public bool playersInTransition, canRunLastAnimation;

    [SerializeField] private Texture whiteCircle, whiteMonster;
    [SerializeField] private RawImage rayImg;

    [SerializeField] private AnimationClip[] animationNames;

    private int currentIndex = 0;

    private Animation anim;
    private int doorKeyTP;
    private Transform doorPosition;
    private GameObject mainCamera;
    private float pressedTime = 0.25f, counterPressedTime;
    private MonsterMove monsterMove;

    private bool controlAlphaOffset = false, canStartCountingInput = false, fastTrans;

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
        rayImg.texture = whiteCircle;

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        monsterMove = GameObject.FindGameObjectWithTag("Player").GetComponent<MonsterMove>();

        anim = GetComponent<Animation>();

        doorKeyTP = 0;
        counterPressedTime = 0.0f;
        playersInTransition = false;
        canRunLastAnimation = false;
        canStartCountingInput = false;
        fastTrans = false;
    }

    private void Update()
    {
        if (canStartCountingInput)
        {
            if (Input.GetKey(KeyCode.E))
            {
                counterPressedTime += Time.deltaTime;

                if (counterPressedTime >= pressedTime)
                {
                    counterPressedTime = 0.0f;
                    canStartCountingInput = false;
                    ExecuteFirstTransitionPressed();
                }
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                if (counterPressedTime < pressedTime)
                {
                    counterPressedTime = 0.0f;
                    fastTrans = true;
                    canStartCountingInput = false;
                    ExecuteTransition();
                }
            }
        }

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
        DoorBase.OnStartCamTransition += DoorInteracted;
    }

    private void OnDisable()
    {
        DoorBase.OnStartCamTransition -= DoorInteracted;
    }

    private void DoorInteracted(int doorKey, Transform doorTransform)
    {
        canStartCountingInput = true;
        playersInTransition = true;
        doorKeyTP = doorKey;
        doorPosition = doorTransform;
    }

    private void ExecuteTransition()
    {
        if (anim == null || animationNames.Length == 0) return;

        rayImg.texture = whiteMonster;

        string firstAnim = animationNames[3].name;
        anim.Play(firstAnim);
    }

    private void ExecuteFirstTransitionPressed()
    {
        if (anim == null || animationNames.Length == 0) return;

        string firstAnim = animationNames[0].name;
        anim.Play(firstAnim);

        currentIndex += 1;
    }

    public void ChangePlayerPosition() // Mitad 3a y Mitad sin press
    {
        if (fastTrans)
        {
            float zAxis = mainCamera.transform.position.z;
            mainCamera.transform.position = new Vector3(doorPosition.position.x, doorPosition.position.y, zAxis);
        }

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

    public void AnimationFinished() // Final 3a y Final sin press
    {
        IntroRoomsManager.Instance.StartRoomAnim();

        if (fastTrans)
            fastTrans = false;

        playersInTransition = false;

        monsterMove.ZeroMoveDir();
        monsterMove.FreezeAnimation();

        rayImg.texture = whiteCircle;
        ResetAnimationIndex();
    }

    private void ResetAnimationIndex()
    {
        currentIndex = 0;
    }
}