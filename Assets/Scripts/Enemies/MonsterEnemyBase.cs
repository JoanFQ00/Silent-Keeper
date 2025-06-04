using UnityEngine;

public class MonsterEnemyBase : MonoBehaviour
{
    private int counterTriggers;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        counterTriggers = 0;

        animator.SetTrigger("Right");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TriggerMonsterEnemy"))
        {
            switch (counterTriggers)
            {
                case 0:
                    collision.gameObject.SetActive(false);
                    animator.SetTrigger("Down");
                    break;
                case 1:
                    collision.gameObject.SetActive(false);
                    animator.SetTrigger("Left");
                    break;
                case 2:
                    collision.gameObject.SetActive(false);
                    animator.SetTrigger("Up");
                    break;
                case 3:
                    collision.gameObject.SetActive(false);
                    animator.SetTrigger("Right");
                    break;
                default:
                    break;
            }

            counterTriggers++;
        }
    }
}