using UnityEngine;
using static HeadHUD;

public class RoomCameraBase : MonoBehaviour
{
    [SerializeField] private bool isCameraStatic = false;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private AnimationClip roomClip = null;

    private bool playerHasEntered;

    private void OnEnable()
    {
        Lever.OnKidLeverActivated += OnKidLeverActivated;
    }

    private void OnDisable()
    {
        Lever.OnKidLeverActivated -= OnKidLeverActivated;
    }

    private void Start()
    {
        playerHasEntered = false;
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsPlayer(collision)) return;

        if (!playerHasEntered)
        {
            playerHasEntered = true;

            if (isCameraStatic)
            {
                CameraFollow.Instance.ChangeTarget(transform);

                if (HeadHUD.Instance != null)
                    HeadHUD.Instance.OnFocusChanged(FocusTarget.None);
            }

            CameraFollow.Instance.SetRoomCollider(GetComponent<Collider2D>());
            IntroRoomsManager.Instance.SetAnimToPlay(roomClip);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsPlayer(collision)) return;

        if (playerHasEntered)
        {
            playerHasEntered = false;

            if (isCameraStatic)
            {
                CameraFollow.Instance.ResetTarget();
            }
        }
    }

    private bool IsPlayer(Collider2D col)
    {
        return ((1 << col.gameObject.layer) & playerLayer) != 0;
    }

    private void OnKidLeverActivated(AnimationClip circusAnim)
    {
        IntroRoomsManager.Instance.SetAnimToPlay(circusAnim);
        IntroRoomsManager.Instance.StartRoomAnim();
    }
}