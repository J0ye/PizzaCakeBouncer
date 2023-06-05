using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public string playerPrefabName;

    public Vector3 cardSpawnPosition;

    public GameState gameState;

    [SerializeField]
    private Card[] cardTypes;
    [SerializeField]
    private int timeBetweenCards = 3;
    [SerializeField]
    private int timeAfterRoundEnd = 5;

    /// <summary>
    /// The card that is currently wanted
    /// </summary>
    private int wantedCardIndex = 0;

    /// <summary>
    /// Card that is currently in play
    /// </summary>
    private GameObject currentCardInPlay;

    private float timer = 0;

    private void Start()
    {
        if(instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        switch (gameState)
        {
            case GameState.CyclingCards:
                if (timer >= timeBetweenCards)
                {
                    ServerSpawnNextCard();
                    timer = 0;
                }
                break;
            case GameState.WaitingForNextRound:
                if (timer >= timeAfterRoundEnd)
                {
                    RestartRound();
                    timer = 0;
                }
                break;
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

    public void HandlePlayerInteraction()
    {
        // TODO: Break the game loop when user tapped the card
        // Either they were correct or punish them if they are wrong
    }

    /// <summary>
    /// Grabs a random card and displays it for the players to tap on
    /// Card probability is weighted by the amount in the card scriptable object
    /// </summary>
    public void ServerSpawnNextCard()
    {
        // Get Random card
        int randomIndex = Random.Range(0, cardTypes.Length);
        RpcSpawnCard(randomIndex);
        //PhotonNetwork.Instantiate("", new Vector2(0, 0), Quaternion.identity);
    }

    public void RpcSpawnCard(int cardIndex)
    {
        Destroy(currentCardInPlay);
        currentCardInPlay = Instantiate(cardTypes[cardIndex].gameObject, cardSpawnPosition, Quaternion.identity);

        CycleWantedCard();
    }

    private void RestartRound()
    {
        ServerSpawnNextCard();
    }

    /// <summary>
    /// Increase the wanted card index by one and reset it if it exceeds the array bounds
    /// Then display the wanted card in UI
    /// </summary>
    private void CycleWantedCard()
    {
        wantedCardIndex++;
        if (wantedCardIndex >= cardTypes.Length)
        {
            wantedCardIndex = 0;
        }

        // TODO: Display wanted card in UI
    }
}

public enum GameState
{
    CyclingCards,
    WaitingForNextRound,
}
