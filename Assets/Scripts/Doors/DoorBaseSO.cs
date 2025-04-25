using UnityEngine;

[CreateAssetMenu(fileName = "Door", menuName = "Element/Door")]
public class DoorBaseSO : ScriptableObject
{
    public Sprite doorImage;
    public bool isZoneDoor;
    public bool isFinalDoor;
}