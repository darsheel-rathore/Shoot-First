using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Panels

    [SerializeField] private GameObject nickNamePanel;
    [SerializeField] private GameObject connectionPanel;
    [SerializeField] private GameObject gameTypePanel;
    [SerializeField] private GameObject joinCreateRoomPanel;

    #endregion

    [SerializeField] private TextMeshProUGUI regionText;
    [SerializeField] private TextMeshProUGUI connectionStatusText;

    private void Start() 
    {
        EnablePanel(nickNamePanel.name);
    }

    private void Update()
    {
        UpdateNetworkStatus();
    }


    private void EnablePanel(string panelName)
    {
        nickNamePanel.SetActive(nickNamePanel.name == panelName);
        connectionPanel.SetActive(connectionPanel.name == panelName);
        gameTypePanel.SetActive(gameTypePanel.name == panelName);
        joinCreateRoomPanel.SetActive(joinCreateRoomPanel.name == panelName);
    }

    private void UpdateNetworkStatus()
    {
        connectionStatusText.text = "Status: " + PhotonNetwork.NetworkClientState.ToString();
    }


    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {
        connectionPanel.SetActive(false);
        regionText.text = "Region: " + PhotonNetwork.CloudRegion;
    }

    #endregion

}
