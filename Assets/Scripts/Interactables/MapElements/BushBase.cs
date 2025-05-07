using UnityEngine;

public class BushBase : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    private Color hiddenColor;

    private void Start()
    {
        Color bushColor = GetComponent<SpriteRenderer>().color;
        hiddenColor = new Color(bushColor.r, bushColor.g, bushColor.b, 0.4f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0) 
        { 
            if (collision.transform.parent.CompareTag("Kid"))
            {
                var kid = collision.transform.parent.GetComponent<KidMachine>();
                kid.bushCounter++;
                UpdateKidVisibility(kid, collision.transform.parent.GetComponent<SpriteRenderer>());
            }
            else if (collision.transform.parent.CompareTag("Player"))
            {
                var monster = collision.transform.parent.GetComponent<MonsterMove>();
                monster.bushCounter++;
                UpdateMonsterVisibility(monster, collision.transform.parent.GetComponent<SpriteRenderer>());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            if (collision.transform.parent.CompareTag("Kid"))
            {
                var kid = collision.transform.parent.GetComponent<KidMachine>();
                kid.bushCounter = Mathf.Max(0, kid.bushCounter - 1);
                UpdateKidVisibility(kid, collision.transform.parent.GetComponent<SpriteRenderer>());
            }
            else if (collision.transform.parent.CompareTag("Player"))
            {
                var monster = collision.transform.parent.GetComponent<MonsterMove>();
                monster.bushCounter = Mathf.Max(0, monster.bushCounter - 1);
                UpdateMonsterVisibility(monster, collision.transform.parent.GetComponent<SpriteRenderer>());
            }
        }
    }

    private void UpdateKidVisibility(KidMachine kid, SpriteRenderer sr)
    {
        if (kid.bushCounter > 0)
        {
            sr.color = hiddenColor;
            kid.ChangeHiddenState(KidMachine.States.HiddenInBush);
        }
        else
        {
            sr.color = kid.originalColor;
            kid.ChangeHiddenState(KidMachine.States.NotHiddenInBush);
        }
    }

    private void UpdateMonsterVisibility(MonsterMove monster, SpriteRenderer sr)
    {
        if (monster.bushCounter > 0)
        {
            sr.color = hiddenColor;
            monster.ChangeHiddenState(KidMachine.States.HiddenInBush);
        }
        else
        {
            sr.color = monster.originalColor;
            monster.ChangeHiddenState(KidMachine.States.NotHiddenInBush);
        }
    }
}