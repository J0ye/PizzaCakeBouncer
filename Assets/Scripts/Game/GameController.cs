using Photon.Pun;
using UnityEngine;
using System.Collections;
using TMPro;
public class GameController : MonoBehaviourPun
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
    private float timeForVictory = 3;
    [Tooltip("Determines how many cards are drawn until the game is over.")]
    [SerializeField]
    private int deckSize = 5;

    /// <summary>
    /// The card that is currently wanted
    /// </summary>
    private int wantedCardIndex = 0;

    /// <summary>
    /// Card that is currently in play
    /// </summary>
    private GameObject currentCardInPlay;
    private float cardTimer = 0f;
    private int points = 0;
    /// <summary>
    /// Increases everytime a new card is drawn. Reset at the beginning of a game.
    /// </summary>
    private int cardsDrawnCounter = 0;
    private bool isPlaying = false;

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
        // Start new Game
        photonView.RPC(nameof(RestartRound), RpcTarget.All);
    }

    private void Update()
    {
        if(isPlaying)
        {
            cardTimer += Time.deltaTime; 
            // Only the master client controls the game loop
            if (PhotonNetwork.IsMasterClient && isPlaying)
            {
                switch (gameState)
                {
                    case GameState.CyclingCards:
                        if (cardTimer >= timeBetweenCards)
                        {
                            ServerSpawnNextCard();
                        }
                        break;
                }

            }
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
        if (currentCard.displayName == GetWantedCard().displayName && isPlaying)
        {
            // wanted card is equal to current card in play
            points++;
            WriteToGameInfo("Yes, Good Job.");
            currentCard.SetColor(Color.green);
            if (PhotonNetwork.IsMasterClient)
            {
                HaltGame();
            }
            else
            {
                photonView.RPC(nameof(HaltGame), RpcTarget.MasterClient); // ask master to pause game so no new cards are spawned
            }
        }
        else if(isPlaying)
        {
            // Checked card but it was wrong
            points--;
            currentCard.SetColor(Color.red);
            WriteToGameInfo("Nope, Wrong Card");
        }
        else
        {
            // Game has already been halted, other player touched wanted card first
            currentCard.SetColor(Color.grey);
            WriteToGameInfo("You Are Too Late");
        }
    }

    [PunRPC]
    public void HaltGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Pause game for everyone
            photonView.RPC(nameof(RPCSetIsPlaying), RpcTarget.All, false);
            // Continue game after t
            Invoke(nameof(ContinueGame), timeForVictory);
        }
    }

    /// <summary>
    /// Called while spawning a new card.
    /// </summary>
    [PunRPC]
    public void EndGame()
    {

    }

    /// <summary>
    /// Called by non-master clients to ask the master to spawn a new card.
    /// </summary>
    [PunRPC]
    public void RequestSpawnCard()
    {
        ServerSpawnNextCard();
    }

    [PunRPC]
    public void RestartRound()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ServerSpawnFirstCard();
        }
        isPlaying = true;
        wantedCardIndex = 0;
        WriteGameStatus();
    }

    public void ContinueGame()
    {
        // Continue game for everyone
        photonView.RPC(nameof(RPCSetIsPlaying), RpcTarget.All, true);
        ServerSpawnNextCard();
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

    public void ServerSpawnFirstCard()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int randomIndex = Random.Range(0, cardTypes.Length); // Get Random card
            photonView.RPC(nameof(RpcSpawnCardWithoutCycle), RpcTarget.All, randomIndex); // Call everybody to draw first and set wanted to 0
        }
        else
        {
            // Do nothing if client
        }

    }

    /// <summary>
    /// Grabs a random card and displays it for the players to tap on.
    /// Card probability is weighted by the amount in the card scriptable object.
    /// Also adds to wantedCardIndex
    /// </summary>
    public void ServerSpawnNextCard()
    {   
        if(PhotonNetwork.IsMasterClient)
        {
            int randomIndex = Random.Range(0, cardTypes.Length); // Get Random card
            photonView.RPC(nameof(RpcSpawnCard), RpcTarget.All, randomIndex); // Call everybody to discard old card and draw new
        }
        else
        {
            // Do nothing if client
        }
    }

    /// <summary>
    /// Called so each player discard the last card and draws a new.
    /// RPCSpawnCard instead of Photon.Instaniate so Destroy(CurrentCard) happens on each instance.
    /// </summary>
    /// <param name="cardIndex"></param>
    [PunRPC]
    public void RpcSpawnCard(int cardIndex)
    {
        SpawnCard(cardIndex);

        CycleWantedCard();
    }

    [PunRPC]
    public void RpcSpawnCardWithoutCycle(int cardIndex)
    {
        SpawnCard(cardIndex);
    }

    private void SpawnCard(int cardIndex)
    {
        if(cardsDrawnCounter >= deckSize)
        {
            EndGame();
            return;
        }

        cardTimer = 0; // New card, new time
        Destroy(currentCardInPlay);
        currentCardInPlay = Instantiate(cardTypes[cardIndex].gameObject, cardSpawnPosition, Quaternion.identity);
        cardsDrawnCounter++;
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

    [PunRPC]
    public void RPCSetIsPlaying(bool newState)
    {
        isPlaying = newState;
    }

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
