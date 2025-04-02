public interface ISkill
{
    void Execute(SkillManager.SkillNumber numSkill, SkillBase skill);
    void UpdateSkill();
}