public class LocaCola : ISkill
{
    public void Execute(SkillManager.SkillNumber numSkill, SkillBase skill)
    {
        if (numSkill == SkillManager.SkillNumber.SKILL1)
        {
            SkillManager.Instance.canUpdateSkill1 = true;
        }
        else
        {
            SkillManager.Instance.canUpdateSkill2 = true;
        }
    }

    public void UpdateSkill()
    {
        
    }
}
