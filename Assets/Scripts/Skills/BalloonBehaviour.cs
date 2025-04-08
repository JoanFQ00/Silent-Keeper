using UnityEngine;

public class BalloonBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || (collision.gameObject.layer == LayerMask.NameToLayer("Wall") && collision.gameObject.GetComponent<Collider2D>().isTrigger == false))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                transform.parent.gameObject.GetComponentInParent<Ballooned>().BallonHasEnemyCollided(collision.gameObject);

            transform.parent.gameObject.GetComponentInParent<Ballooned>().bloonTime = 0f;
            gameObject.SetActive(false);
        }
    }
}