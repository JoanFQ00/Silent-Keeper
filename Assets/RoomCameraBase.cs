using UnityEngine;

public class RoomCameraBase : MonoBehaviour
{
    [SerializeField] private bool isCameraStatic = false;

    private bool playerHasEntered;

    private void Start()
    {
        playerHasEntered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerHasEntered)
        {
            playerHasEntered = true;

            if (isCameraStatic)
            {
                CameraFollow.Instance.ChangeTarget(gameObject.transform); //PODEMOS PONER UN EMPTY DENTRO DE LA HABITACIÓN PARA CONTROLAR EL CENTRO DE LA CÁMARA
            }

            CameraFollow.Instance.SetRoomCollider(gameObject.GetComponent<PolygonCollider2D>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerHasEntered)
        {
            playerHasEntered = false;

            if (isCameraStatic)
                CameraFollow.Instance.ResetTarget();
        }
    }
}