using Photon.Pun;
using UnityEngine;
using System.Collections;
using TMPro;
using Mirror;

public class GameController : NetworkBehaviour
{
    public static GameController instance;

    public Vector3 cardSpawnPosition;
    public GameState gameState;
    public TMP_Text gameInfoText;

    [SerializeField]
    private Card[] cardTypes;
    [SerializeField]
    private int timeBetweenCards = 3;
    [Tooltip("How long the game is paused if a player touches a wanted card")]
    [SerializeField]
    private float timeForVictory= 3;
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
    private int points = 0;
    private float cardTimer = 0f;

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
        WriteGameStatus();

    }

    public void StartGame()
    {

    }

    private void Update()
    {
        cardTimer += Time.deltaTime;
        if (isServer)
        {
            GameLoop();
        }
    }

    [Server]
    private void GameLoop()
    {
        // Only the master client controls the game loop
        switch (gameState)
        {
            case GameState.CyclingCards:
                if (cardTimer >= timeBetweenCards)
                {
                    ServerSpawnNextCard();
                }
                break;
            case GameState.WaitingForNextRound:
                if (cardTimer >= timeAfterRoundEnd)
                {
                    ServerSpawnNextCard();
                }
                break;
        }
    }

    /// <summary>
    /// Is called by cards after player interaction.
    /// </summary>
    /// <returns></returns>
    public void HandlePlayerInteraction()
    {
        Card currentCard = currentCardInPlay.GetComponent<Card>();
        // I think we need something better than to compare names. Works for now
        if (currentCard.displayName == GetWantedCard().displayName)
        {
            // wanted card is equal to current card in play
            points++;
            WriteToGameInfo("Yes, Good Job.");
            currentCard.SetColor(Color.green);
        }
        else
        {
            // Checked card but it was wrong
            points--;
            currentCard.SetColor(Color.red);
            WriteToGameInfo("Nope, Wrong Card");
        }
        //else
        //{
        //    // Game has already been halted, other player touched wanted card
        //    currentCard.SetColor(Color.grey);
        //    WriteToGameInfo("You Are Too Late");
        //}
    }

    #region card logic
    /// <summary>
    /// The card that players have to currently look for
    /// </summary>
    /// <returns></returns>
    public Card GetWantedCard()
    {
        return cardTypes[wantedCardIndex];
    }

    /// <summary>
    /// Grabs a random card and displays it for the players to tap on.
    /// Card probability is weighted by the amount in the card scriptable object.
    /// Also adds to wantedCardIndex
    /// </summary>
    public void ServerSpawnNextCard()
    {
        int randomIndex = Random.Range(0, cardTypes.Length); // Get Random card
        RpcSpawnCard(randomIndex);
    }

    /// <summary>
    /// Called so each player discard the last card and draws a new.
    /// RPCSpawnCard instead of Photon.Instaniate so Destroy(CurrentCard) happens on each instance.
    /// </summary>
    /// <param name="cardIndex"></param>
    [ClientRpc]
    public void RpcSpawnCard(int cardIndex)
    {
        SpawnCard(cardIndex);
        CycleWantedCard();
    }

    private void SpawnCard(int cardIndex)
    {
        cardTimer = 0; // New card, new time
        Destroy(currentCardInPlay);
        currentCardInPlay = Instantiate(cardTypes[cardIndex].gameObject, cardSpawnPosition, Quaternion.identity);
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
        WriteGameStatus();
    }
    #endregion

    #region Game Status
    private void WriteGameStatus()
    {
        string gameStatus = "WANTED: " + GetWantedCard().displayName;
        gameStatus += "\n Points: " + points;
        WriteToGameInfo(gameStatus);
    }

    private void WriteToGameInfo(string txt)
    {
        if (gameInfoText != null)
        {
            gameInfoText.text = txt;
        }
    }

    #endregion
}

public enum GameState
{
    CyclingCards,
    WaitingForNextRound,
}
