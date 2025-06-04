using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Kid") || collision.CompareTag("Player"))
        {
            GameManager.Instance.currentSpawn = this.transform;
            Destroy(gameObject);
        }
    }
}