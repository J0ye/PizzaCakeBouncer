using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using TMPro;
using System.Linq;


/// <summary>
/// Controls connecting and sessions. Does not care for any player specific functions
/// </summary>
public class ConncectionManager : MonoBehaviourPunCallbacks
{
    public static ConncectionManager instance;
    public bool writeStatusToConsole;

    public UnityEvent OnEnterRoom = new UnityEvent();
    public UnityEvent OnOtherPlayerEnterRoom = new UnityEvent();
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

    // Start is called before the first frame update
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

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = RandomValues.INSTANCE().GetRandomString();
        WriteStatus("Nickname: " + PhotonNetwork.NickName);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        StartCoroutine(JoinStandardRoom());
    }

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
}

static class NewPhotonPlayerMethods
{
    public static void CallByName(this Player player)
    {
        Debug.Log("And before came " + player.NickName);
    }
}