using UnityEngine;

public class FinalZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Kid")
        {
            GameManager.Instance.ActiveFinalPanel();
        }
    }
}