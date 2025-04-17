using UnityEngine;

public class ReyHielo : MonoBehaviour, ISkill
{
    [Header("Skill (ScriptableObject)")]
    public SkillBase thisSkill;

    [Header("Skill Variables")]
    public float timerDecrement = 0.5f;

    public void Execute(GameObject monster)
    {
        GameManager.Instance.timerSpeedMultiplier = timerDecrement;
    }

    public void UpdateSkill() { }

    public SkillBase GetSkill()
    {
        return thisSkill;
    }

    public void SetInitialHasFinished() { }

    public bool HasFinished()
    {
        return false;
    }
}