using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

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
    public string[] availableColors;

    /// <summary>
    /// All available sprites for the card type
    /// </summary>
    public Sprite[] availableSprites;

    [SerializeField]
    private GameObject backGround;
    [SerializeField]
    private GameObject cardSprite;

    // Start is called before the first frame update
    void Start()
    {
        SetRandomSprite();
        SetRandomColor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Sets a random background color for the current card
    /// </summary>
    public void SetRandomSprite()
    {
        int randomIndex = Random.Range(0, availableSprites.Length);
        cardSprite.GetComponent<SpriteRenderer>().sprite = availableSprites[randomIndex];
    }

    /// <summary>
    /// Sets a random background color for the current card
    /// </summary>
    public void SetRandomColor() 
    {
        int randomIndex = Random.Range(0, availableColors.Length);
        ColorUtility.TryParseHtmlString(availableColors[randomIndex], out Color color);
        backGround.GetComponent<SpriteRenderer>().color = color;
    }
}
