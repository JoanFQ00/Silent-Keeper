using UnityEngine;

public class ReyHielo : MonoBehaviour, ISkill
{
    [Header("Skill (ScriptableObject)")]
    public SkillBase thisSkill;

    [Header("Skill Variables")]
    public float timerDecrement = 0.5f; // 0.5 = el tiempo pasa a la mitad de velocidad

    //private bool isPassiveActivated = false;

    public void Execute(GameObject monster)
    {
        //isPassiveActivated = !isPassiveActivated;

        //if (isPassiveActivated)
        //{
        //    GameManager.Instance.timerSpeedMultiplier = timerDecrement;
        //}
        //else
        //{
        //    GameManager.Instance.timerSpeedMultiplier = 1f;
        //}

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