using UnityEngine;

public class BushBase : MonoBehaviour
{
    private Color hiddenColor;

    private void Start()
    {
        Color bushColor = GetComponent<SpriteRenderer>().color;
        hiddenColor = new Color(bushColor.r, bushColor.g, bushColor.b, 0.8f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Kid"))
        {
            var kid = collision.GetComponent<KidMachine>();
            kid.bushCounter++;
            UpdateKidVisibility(kid, collision.GetComponent<SpriteRenderer>());
        }
        else if (collision.CompareTag("Player"))
        {
            var monster = collision.GetComponent<MonsterMove>();
            monster.bushCounter++;
            UpdateMonsterVisibility(monster, collision.GetComponent<SpriteRenderer>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Kid"))
        {
            var kid = collision.GetComponent<KidMachine>();
            kid.bushCounter = Mathf.Max(0, kid.bushCounter - 1);
            UpdateKidVisibility(kid, collision.GetComponent<SpriteRenderer>());
        }
        else if (collision.CompareTag("Player"))
        {
            var monster = collision.GetComponent<MonsterMove>();
            monster.bushCounter = Mathf.Max(0, monster.bushCounter - 1);
            UpdateMonsterVisibility(monster, collision.GetComponent<SpriteRenderer>());
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