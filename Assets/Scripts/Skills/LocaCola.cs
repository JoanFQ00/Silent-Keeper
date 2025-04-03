using UnityEngine;

public class LocaCola : MonoBehaviour, ISkill
{
    [Header("Skill (ScriptableObject)")]
    public SkillBase thisSkill;

    [Header("Skill Variables")]
    public float exrtaSpeed;
    public float buffTime;

    private float defaultBuffTime;
    private GameObject player;
    private bool finishedSkill;

    public void Execute(GameObject monster)
    {
        defaultBuffTime = buffTime;
        player = monster;
        player.GetComponent<MonsterMove>().SetSpeed(exrtaSpeed);

        finishedSkill = false;
    }

    public void UpdateSkill()
    {
        if (buffTime >= 0f)
        {
            buffTime -= Time.deltaTime;
        }
        else
        {
            buffTime = defaultBuffTime;
            player.GetComponent<MonsterMove>().SetSpeed(1f);

            finishedSkill = true;
        }
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