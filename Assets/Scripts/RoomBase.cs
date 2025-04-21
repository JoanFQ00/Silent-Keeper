using UnityEngine;

public class RoomBase : MonoBehaviour
{
    private bool isSmallRoom;

    public static bool playersAreInRoom;

    private void Start()
    {
        playersAreInRoom = true;
        isSmallRoom = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSmallRoom)
        {
            if (collision.tag == "Player" || collision.tag == "Kid")
            {
                playersAreInRoom = true;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().ChangeTarget(gameObject.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isSmallRoom)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Kid"))
            {
                playersAreInRoom = false;

                GameObject player = GameObject.FindGameObjectWithTag("Player");
                GameObject kid = GameObject.FindGameObjectWithTag("Kid");
                GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");

                if (player != null && kid != null && cameraObject != null)
                {
                    Collider2D roomCollider = GetComponent<Collider2D>();
                    if (!roomCollider.bounds.Contains(player.transform.position) &&
                        !roomCollider.bounds.Contains(kid.transform.position))
                    {
                        CameraFollow camFollow = cameraObject.GetComponent<CameraFollow>();
                        if (camFollow != null)
                            camFollow.ChangeTarget(player.transform);
                    }
                }
            }
        }
    }
}