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

                if (!GetComponent<Collider2D>().bounds.Contains(player.transform.position) &&
                    !GetComponent<Collider2D>().bounds.Contains(kid.transform.position))
                {
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().ChangeTarget(player.transform);
                }
            }
        }
    }
}