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
    private bool finishedSkill, ballonHasCollided;

    private void Start()
    {
        ballonHasCollided = false;
    }

    private void OnEnable()
    {
        BalloonBehaviour.OnBallonImpacted += OnBallonImpacted;
        BalloonBehaviour.OnHasFinished += OnBallonFinished;
    }

    private void OnDisable()
    {
        BalloonBehaviour.OnBallonImpacted -= OnBallonImpacted;
        BalloonBehaviour.OnHasFinished -= OnBallonFinished;
    }

    private void OnBallonImpacted()
    {
        ballonHasCollided = true;
    }

    private void OnBallonFinished()
    {
        bloonTime = defaultBuffTime;
        balloon.SetActive(false);
        BalloonBox.AddBallon();
        finishedSkill = true;
        ballonHasCollided = false;
    }

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
        balloon.GetComponent<BalloonBehaviour>().hasTimeFinished = false;

        finishedSkill = false;
    }

    public void UpdateSkill()
    {
        if (bloonTime >= 0f && !ballonHasCollided)
        {
            balloon.transform.Translate(ballonDirection * bloonSpeed * Time.deltaTime, Space.World);
            bloonTime -= Time.deltaTime;
        }
        else
        {
            if (!ballonHasCollided)
            {
                balloon.GetComponent<BalloonBehaviour>().hasTimeFinished = true;
                balloon.GetComponent<Animator>().SetTrigger("End");

                balloon.transform.Translate(ballonDirection * bloonSpeed * Time.deltaTime, Space.World);
                bloonTime -= Time.deltaTime;
            }
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