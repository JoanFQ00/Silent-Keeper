using UnityEngine;

public class RoomCameraBase : MonoBehaviour
{
    [SerializeField] private bool isCameraStatic = false;
    [SerializeField] private LayerMask playerLayer;

    private bool playerHasEntered;

    private void Start()
    {
        playerHasEntered = false;
        GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsPlayer(collision)) return;

        if (!playerHasEntered)
        {
            playerHasEntered = true;

            if (isCameraStatic)
            {
                // Puedes usar un Empty como target para centrar la cámara en una zona específica.
                CameraFollow.Instance.ChangeTarget(transform);
            }

            CameraFollow.Instance.SetRoomCollider(GetComponent<PolygonCollider2D>());
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
}