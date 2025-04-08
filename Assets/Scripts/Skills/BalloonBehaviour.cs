using UnityEngine;

public class BalloonBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            if (collision.tag == "Enemy")
            {

            }

            gameObject.SetActive(false);
        }
    }
}