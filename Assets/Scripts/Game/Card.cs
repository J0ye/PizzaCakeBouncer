using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using DG.Tweening;

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

    public bool isInteractable { get; private set; }

    [Header("Animation")]
    public float animationDuration = 1f;

    [SerializeField] //Why serialize this??
    private GameObject backGround;
    [SerializeField] //Why serialize this??
    private GameObject cardSprite;

    private TouchObjectScript tos;

    void Awake()
    {
        SetRandomSprite();
        SetRandomColor();
        tos = GetComponent<TouchObjectScript>();
        tos.onDown.AddListener(TouchCard);
        isInteractable = false;
        Invoke(nameof(SetInteractable), animationDuration);
        PlayDrawAnimation();
    }

    public void TouchCard()
    {
        if(isInteractable)
        {
            PlayTapAnimation();
            GameController.instance.HandlePlayerInteraction();
        }
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
        int randomIndex = Random.Range(0, availableColors.Count);
        backGround.GetComponent<SpriteRenderer>().color = availableColors[randomIndex];
    }
    public void SetColor(Color c)
    {
        backGround.GetComponent<SpriteRenderer>().color = c;
    }

    #region Animations
    public void PlayTapAnimation()
    {
        Vector3 strength = new Vector3(0.4f, 0.4f, 0.4f);
        transform.DOPunchScale(strength, animationDuration / 10); // Duration should be a fraction of normal duration. Taps are very short
    }

    public void PlayDrawAnimation()
    {
        transform.position = GetCardPositionOffscreen(1); // Hide the card off the right side of the screen
        PlayRotateToRandomAngle(-7f, 7f);
        transform.DOMove(Vector3.zero, animationDuration);
    }

    public void PlayDiscardAnimation()
    {
        PlayRotateToRandomAngle(-30f, 30f);
        transform.DOMove(GetCardPositionOffscreen(0), animationDuration);
    }

    public void PlayTakeCardAnimation()
    {
        transform.DOMove(GetCardPositionOffscreen(3), animationDuration);
    }

    public void PlayGiveCardAnimation()
    {
        PlayRotateToRandomAngle(-2f, 2f);
        transform.DOMove(GetCardPositionOffscreen(2), animationDuration);
    }

    public void PlayRotateToRandomAngle(float minAngle, float maxAngle)
    {
        Vector3 currentAngles = transform.rotation.eulerAngles;
        Vector3 randomAngle = new Vector3(0, 0, Random.Range(minAngle, maxAngle));
        transform.DORotate(currentAngles + randomAngle, animationDuration);
    }

    /// <summary>
    /// Position the card offscreen. 
    /// 0 = left
    /// 1 = right
    /// 2 = top
    /// 3 = bottom
    /// default = right
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Vector3 GetCardPositionOffscreen(int direction)
    {
        switch(direction)
        {
            case 0:
                // Left
                return transform.position + Vector3.left * 10;
            case 1:
                // Right
                return transform.position + Vector3.right * 10;
            case 2:
                // Top
                return transform.position + Vector3.up * 10;
            case 3:
                // Bottom
                return transform.position + Vector3.down * 10;
            default:
                // Default is right
                return transform.position + Vector3.right * 10;

        }
    }
    #endregion

    public void SetInteractable()
    {
        isInteractable = true;
        // Move background image further back in order
        transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 0;
        // Move image of card further back in order
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public void SetUnInteractable()
    {
        isInteractable = false;
        // Move background image further back in order
        transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = -4;
        // Move image of card further back in order
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = -4;
    }
}
