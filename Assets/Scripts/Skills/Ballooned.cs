using UnityEngine;

public class Ballooned : MonoBehaviour, ISkill
{
    [Header("Skill (ScriptableObject)")]
    public SkillBase thisSkill;

    [Header("Skill Variables")]
    public float buffTime;

    private float defaultBuffTime;
    //private GameObject player;
    private bool finishedSkill;

    public void Execute(GameObject monster)
    {
        defaultBuffTime = buffTime;
        //player = monster;

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