using UnityEngine;

[CreateAssetMenu(fileName = "SkillBase", menuName = "Scriptable Objects/SkillBase")]
public class SkillBase : ScriptableObject
{
    enum SkillType
    {
        ACTIVE = 0,
        PASIVE
    }

    enum ReloadType
    {
        ROOM = 0,
        ZONE
    }

    private SkillType currentSkillType;
    private ReloadType currentReloadType;
    private string skillName, skillDescription;
    private uint uses;
}
