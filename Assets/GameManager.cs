using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE;

    public string playerPrefabName;
    public Vector2 xBounds = Vector2.zero;
    public Vector2 yBounds = Vector2.zero;
    public int margin = 1;

    private PositionList positionList;

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

        positionList = new PositionList(margin, xBounds, yBounds);
    }

    public void SpawnPlayer()
    {
        PhotonNetwork.Instantiate(playerPrefabName, positionList.GetRandomPositionIn2D(), Quaternion.identity);
    }
}
