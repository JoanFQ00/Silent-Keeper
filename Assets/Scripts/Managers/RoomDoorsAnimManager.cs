using UnityEngine;
using System.Collections.Generic;

public class RoomDoorsAnimManager : MonoBehaviour
{
    [Header("Doors")]
    public List<DoorBase> roomDoors;
    
    [Header("Walls")]
    public List<DoorBase> roomWalls;

    //DESDE LA ANIMACIÓN
    public void OnEventChanged()
    {
        IntroRoomsManager.Instance.OnListUpdated(roomDoors, roomWalls);
    }
}