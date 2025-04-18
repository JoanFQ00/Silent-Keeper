using UnityEngine;

public interface ISkill
{
    void Execute(GameObject player);
    void UpdateSkill();
    SkillBase GetSkill();
    void SetInitialHasFinished();
    bool HasFinished();
}

public interface IInteractableWithButton
{
    void Interacted();
    void Interacted(GameObject other, GameObject thisObject);
}