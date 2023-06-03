using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using TMPro;
using System.Linq;


/// <summary>
/// Controls connecting and sessions. Does not care for any player specific functions. Connects to the lobby automatically and opens a new room if none is open yet.
/// This script is SINGELTON. Only one per scene.
/// </summary>
public class ConncectionManager : MonoBehaviourPunCallbacks
{
    public static ConncectionManager instance;
    public bool createOrJoinRoomOnConnect = true;

    [Header("Events")]
    public UnityEvent OnEnterRoom = new UnityEvent();
    public UnityEvent OnOtherPlayerEnterRoom = new UnityEvent();

    [Header ("Debug")]
    public bool writeStatusToConsole;
    public TextMeshProUGUI statusOutput;
    public List<int> playerIDs = new List<int>();
    public List<string> players = new List<string>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        WriteStatus("Connecting...");
        if (!PhotonNetwork.ConnectUsingSettings())
        {
            WriteStatus("Not able to connect");
            Debug.LogError("Not able to connect");
        }
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            players.Clear();
            Player[] tempPlayerArray = new Player[PhotonNetwork.CurrentRoom.Players.Count];
            playerIDs = PhotonNetwork.CurrentRoom.Players.Keys.ToList<int>();
            foreach (Player p in PhotonNetwork.CurrentRoom.Players.Values)
            {
                players.Add(p.NickName);
            }
        }
    }

    #region connection events
    // Called when device is online and game is connected to master server
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = RandomValues.INSTANCE().GetRandomString();
        WriteStatus("Nickname: " + PhotonNetwork.NickName);
        PhotonNetwork.JoinLobby();
    }

    // Called when entered lobby
    public override void OnJoinedLobby()
    {
        if (!createOrJoinRoomOnConnect) return;

        StartCoroutine(JoinStandardRoom());
    }

    // Called when entered new game room
    public override void OnJoinedRoom()
    {
        WriteStatus("in room");
        OnEnterRoom.Invoke();       
        WritePlayerCount();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnOtherPlayerEnterRoom.Invoke();
        WritePlayerCount();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        WritePlayerCount();
    }
    #endregion

    /// <summary>
    /// Creates an artificial count down before joining or creating a new room.
    /// </summary>
    /// <returns>returns void</returns>
    private IEnumerator JoinStandardRoom()
    {
        WriteStatus("3");
        for (int i = 3; i > 0; i--)
        {
            WriteStatus(i.ToString());
            yield return new WaitForSeconds(1f);
        }
        print("joining");
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    #region Debug and console functions
    private void WritePlayerCount()
    {
        string temp = "Room name: " + PhotonNetwork.CurrentRoom.Name;
        temp += "; Players: " + PhotonNetwork.CurrentRoom.PlayerCount;
        WriteStatus(temp);
    }

    private void WriteStatus(string text)
    {
        if (statusOutput != null)
        {
            statusOutput.text = text;
        }
        if(writeStatusToConsole)Debug.Log(text);
    }
    #endregion
}