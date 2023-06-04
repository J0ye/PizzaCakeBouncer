using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController INSTANCE;

    public string playerPrefabName;

    [SerializeField]
    private Card[] cardTypes;

    /// <summary>
    /// The card that is currently in looked for
    /// </summary>
    private int wantedCardIndex = 0;

    /// <summary>
    /// Card that is currently in play
    /// </summary>
    private Card currentCardInPlay;

    private void Start()
    {
        if(INSTANCE)
        {
            Destroy(this);
        }
        else
        {
            INSTANCE = this;
        }
    }

    /// <summary>
    /// The card that players have to currently look for
    /// </summary>
    /// <returns></returns>
    public Card GetWantedCard()
    {
        return cardTypes[wantedCardIndex];
    }

    public void SpawnPlayer()
    {
        PhotonNetwork.Instantiate(playerPrefabName, new Vector2(0,0), Quaternion.identity);
    }

    /// <summary>
    /// Grabs a random card and displays it for the players to tap on
    /// Card probability is weighted by the amount in the card scriptable object
    /// </summary>
    public void SpawnNextCard()
    {
        PhotonNetwork.Instantiate("", new Vector2(0, 0), Quaternion.identity);
    }
}
