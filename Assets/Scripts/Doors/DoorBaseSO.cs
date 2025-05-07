using UnityEngine;

[CreateAssetMenu(fileName = "Door", menuName = "Element/Door")]
public class DoorBaseSO : ScriptableObject
{
    public Sprite doorImage, openDoorImage;
    public bool isZoneDoor;
    public bool isFinalDoor;
}