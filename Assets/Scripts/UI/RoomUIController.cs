using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomUIController : MonoBehaviour
{
    public static RoomUIController instance;

    [SerializeField]
    private GameObject playerPanel;

    [SerializeField]
    private GameObject roomPlayerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRoomPlayers()
    {
        Debug.Log("Update room players");
        //delete old entries
        foreach (Transform child in playerPanel.transform)
        {
            Destroy(child.gameObject);
        }

        CustomNetworkRoomManager nw = CustomNetworkRoomManager.singleton as CustomNetworkRoomManager;
        foreach (var slot in nw.roomSlots)
        {
            var player = Instantiate(roomPlayerPrefab, playerPanel.transform);
            // Rename the player
            player.transform.Find("PlayerText").GetComponent<TMP_Text>().text = "Player" + (slot.index + 1);
        }
    }
}
