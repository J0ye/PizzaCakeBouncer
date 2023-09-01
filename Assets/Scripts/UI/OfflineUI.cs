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
        Uri uri = new Uri(ipAddressGO.GetComponent<TMP_InputField>().text);
        NetworkManager.singleton.StartClient(uri);
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
