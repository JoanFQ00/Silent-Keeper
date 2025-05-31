using UnityEngine;
using System.Collections.Generic;

public class RoomDoorsAnimManager : MonoBehaviour
{
    public List<DoorBase> roomDoors;
    private bool onEventExecuted = false;

    public void OnEventChanged()
    {
        if (!onEventExecuted)
        {
            IntroRoomsManager.Instance.OnListUpdated(roomDoors);
            onEventExecuted = true;
        }
    }
}