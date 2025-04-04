using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Door", menuName = "Element/Door")]
public class DoorBase : ScriptableObject
{
    public Sprite doorImage;
    public SkillBase.ReloadType reloadType;
}