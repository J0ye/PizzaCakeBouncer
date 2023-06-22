using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(TouchObjectScript))]
public class Card : MonoBehaviour
{
    /// <summary>
    /// How often should this card spawn
    /// </summary>
    public int probability = 1;

    /// <summary>
    /// The display name of the card
    /// </summary>
    public string displayName;

    /// <summary>
    /// All available background colors
    /// </summary>
    public List<Color> availableColors;

    /// <summary>
    /// All available sprites for the card type
    /// </summary>
    public Sprite[] availableSprites;

    [SerializeField] //Why serialize this??
    private GameObject backGround;
    [SerializeField] //Why serialize this?? Because we are attching it in the inspector. Not sure if there is a better way
    private GameObject cardSprite;

    private TouchObjectScript tos;

    void Awake()
    {
        SetRandomSprite();
        SetRandomColor();
        tos = GetComponent<TouchObjectScript>();
        tos.onDown.AddListener(TouchCard);
    }

    public void TouchCard()
    {
        GameController.instance.HandlePlayerInteraction();
    }

    /// <summary>
    /// Sets a random background color for the current card
    /// </summary>
    public void GetRandomSpriteIndex()
    {
        int randomIndex = Random.Range(0, availableSprites.Length);
        cardSprite.GetComponent<SpriteRenderer>().sprite = availableSprites[randomIndex];
    }

    /// <summary>
    /// Sets a random background color for the current card
    /// </summary>
    public void GetRandomColor() 
    {
        int randomIndex = Random.Range(0, availableColors.Count);
        backGround.GetComponent<SpriteRenderer>().color = availableColors[randomIndex];
    }
    public void SetColor(Color c)
    {
        backGround.GetComponent<SpriteRenderer>().color = c;
    }
}
