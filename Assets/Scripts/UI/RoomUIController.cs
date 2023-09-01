using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUIController : MonoBehaviour
{
    public static RoomUIController instance;

    [SerializeField]
    private GameObject playerPanel;

    [SerializeField]
    private GameObject roomPlayerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddRoomPlayer(string name)
    {
        Instantiate(roomPlayerPrefab, playerPanel.transform);
    }
}
