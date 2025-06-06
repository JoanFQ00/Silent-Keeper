using UnityEngine;

public class CameraMachinAdri : MonoBehaviour
{
    public Transform player;
    public BoxCollider2D cameraZone;
    public Camera mainCamera;

    private bool isPlayerInside = false;

    private void LateUpdate()
    {
        if (player == null || mainCamera == null)
            return;

        Vector3 playerPos = player.position;
        Vector3 camPos = mainCamera.transform.position;

        if (isPlayerInside && cameraZone != null)
        {
            Bounds bounds = cameraZone.bounds;

            float halfHeight = mainCamera.orthographicSize;
            float halfWidth = halfHeight * mainCamera.aspect;

            float minX = bounds.min.x + halfWidth;
            float maxX = bounds.max.x - halfWidth;
            float minY = bounds.min.y + halfHeight;
            float maxY = bounds.max.y - halfHeight;

            Vector3 targetPos = camPos;

            if (bounds.size.x >= halfWidth * 2)
            {
                targetPos.x = Mathf.Clamp(playerPos.x, minX, maxX);
            }
            else
            {
                targetPos.x = bounds.center.x;
            }

            if (bounds.size.y >= halfHeight * 2)
            {
                targetPos.y = Mathf.Clamp(playerPos.y, minY, maxY);
            }
            else
            {
                targetPos.y = bounds.center.y;
            }

            mainCamera.transform.position = new Vector3(targetPos.x, targetPos.y, camPos.z);
        }
        else
        {
            mainCamera.transform.position = new Vector3(playerPos.x, playerPos.y, camPos.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == player)
            isPlayerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == player)
            isPlayerInside = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (cameraZone != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(cameraZone.bounds.center, cameraZone.bounds.size);
        }
    }
}
