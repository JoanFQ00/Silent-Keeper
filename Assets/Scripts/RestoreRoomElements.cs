using UnityEngine;
using System.Collections.Generic;

public class RestoreRoomElements : MonoBehaviour
{
    [SerializeField] private List<GameObject> elements;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Kid"))
        {
            foreach (GameObject enemy in elements)
            {
                enemy.GetComponentInChildren<PatrolBehaviour>().ResetCurrentPoint();
                enemy.gameObject.SetActive(true);
            }

            gameObject.SetActive(false);
        }
    }
}