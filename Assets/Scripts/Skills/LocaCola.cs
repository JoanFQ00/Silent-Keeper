using UnityEngine;
using static SkillManager;

public class LocaCola : MonoBehaviour, ISkill
{
    [Header("Skill (ScriptableObject)")]
    public SkillBase thisSkill;

    [Header("Skill Variables")]
    public float exrtaSpeed;
    public float buffTime;

    //private float deafaultSpeed;
    private float defaultBuffTime;
    private GameObject player;
    //private SkillManager.SkillNumber skillNumber;
    private bool finishedSkill;

    public void Execute(GameObject monster)
    {
        //skillNumber = numSkill;
        defaultBuffTime = buffTime;
        player = monster;

        //deafaultSpeed = player.GetComponent<MonsterMove>().GetSpeed();
        player.GetComponent<MonsterMove>().SetSpeed(exrtaSpeed);

        //if (skillNumber == SkillManager.SkillNumber.SKILL1) SkillManager.Instance.canUpdateSkill1 = true;
        //else SkillManager.Instance.canUpdateSkill2 = true;
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

            //if (skillNumber == SkillManager.SkillNumber.SKILL1) SkillManager.Instance.canUpdateSkill1 = false;
            //else SkillManager.Instance.canUpdateSkill2 = false;
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