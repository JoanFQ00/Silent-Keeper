using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool hasToReset = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Kid") || collision.CompareTag("Player"))
        {
            if (hasToReset)
                GameManager.lastCheckpoint = true;

            GameManager.currentSpawn = transform.position;
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}