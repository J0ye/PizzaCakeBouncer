using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{ 
    public List<Color> colors = new List<Color> {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.magenta,
        Color.cyan
    };
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on this GameObject. Please make sure this script is attached to a GameObject with a SpriteRenderer component.");
        }
    }

    // This method changes the color to a random color.
    public void ChangeToRandomColor()
    {
        if(photonView.IsMine)
        {
            if (spriteRenderer != null)
            {
                int randomIndex = Random.Range(0, colors.Count);
                spriteRenderer.color = colors[randomIndex]; 
                photonView.RPC("ChangeSpriteColor", RpcTarget.AllBuffered, spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a);
            }
        }
    }

    [PunRPC]
    public void ChangeSpriteColor(float r, float g, float b, float a)
    {
        spriteRenderer.color = new Color(r, g, b, a);
    }
}
