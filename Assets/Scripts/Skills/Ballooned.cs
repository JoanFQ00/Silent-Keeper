using UnityEngine;

public class Ballooned : MonoBehaviour, ISkill
{
    [Header("Skill (ScriptableObject)")]
    public SkillBase thisSkill;

    [Header("Skill Variables")]
    public float bloonSpeed, bloonTime, stunTime;
    public GameObject balloon;

    private Vector3 mousePos, ballonDirection;

    private float defaultBuffTime;
    private bool finishedSkill;

    public void Execute(GameObject monster)
    {
        defaultBuffTime = bloonTime;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        balloon.transform.position = monster.transform.position;
        ballonDirection = (mousePos - balloon.transform.position).normalized;

        float angle = Mathf.Atan2(ballonDirection.y, ballonDirection.x) * Mathf.Rad2Deg - 90f;
        balloon.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        balloon.SetActive(true);
        finishedSkill = false;
    }

    public void UpdateSkill()
    {
        if (bloonTime >= 0f)
        {
            balloon.transform.Translate(ballonDirection * bloonSpeed * Time.deltaTime, Space.World);
            bloonTime -= Time.deltaTime;
        }
        else
        {
            bloonTime = defaultBuffTime;
            balloon.SetActive(false);
            finishedSkill = true;
        }
    }

    public void BallonHasEnemyCollided(GameObject enemy)
    {
        enemy.GetComponent<EnemyDetector>().SetEnemyState(EnemyDetector.EnemyState.STUNED);
        enemy.GetComponent<EnemyDetector>().SetStunTime(stunTime);
    }

    public SkillBase GetSkill()
    {
        return thisSkill;
    }

    public void SetInitialHasFinished()
    {
        finishedSkill = true;
    }

    public bool HasFinished()
    {
        return finishedSkill;
    }
}