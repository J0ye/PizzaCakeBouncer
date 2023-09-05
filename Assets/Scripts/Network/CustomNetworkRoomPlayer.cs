using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNetworkRoomPlayer : NetworkRoomPlayer
{
    public override void OnClientEnterRoom()
    {
        RoomUIController.instance.UpdateRoomPlayers();
    }

    public override void OnClientExitRoom()
    {
        RoomUIController.instance.UpdateRoomPlayers();
    }

    public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
    {
        base.ReadyStateChanged(oldReadyState, newReadyState);
        RoomUIController.instance.UpdateRoomPlayers();
    }
}
