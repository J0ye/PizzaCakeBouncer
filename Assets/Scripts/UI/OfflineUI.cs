using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfflineUI : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject joinPanel;
    public void StartGame()
    {
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
        GameObject ipAddressGO = joinPanel.transform.Find("IPAddress").gameObject;
        NetworkManager.singleton.networkAddress = ipAddressGO.GetComponent<TMP_InputField>().text;
        NetworkManager.singleton.StartClient();
    }

    public void ViewJoinGameUI()
    {
        TogglePanel(joinPanel);
    }

    void TogglePanel(GameObject go)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        go.SetActive(true);
    }
}
