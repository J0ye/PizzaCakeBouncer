using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndGameUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image background;
    public TMP_Text playerNameText;
    public TMP_Text scoreText;
    public GameObject restartButton;


    public void Awake()
    {
        
    }

    public void SetDetails(string playerName, int score)
    {
        playerNameText.text = playerName;
        scoreText.text = score.ToString();
    }
}
