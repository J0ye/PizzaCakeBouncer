using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNetworkRoomManager : NetworkRoomManager
{
    public override void OnRoomServerAddPlayer(NetworkConnectionToClient conn)
    {
        RoomUIController.instance.UpdateRoomPlayers();
    }

    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
    {
        RoomUIController.instance.UpdateRoomPlayers();
    }
}
