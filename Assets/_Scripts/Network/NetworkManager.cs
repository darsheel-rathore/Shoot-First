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
    [SerializeField] private TextMeshProUGUI nickNameText;
    [SerializeField] private TMP_InputField nickNameInputText;


    // ======================================


    private void Start() 
    {
        EnablePanel(nickNamePanel.name);
    }

    private void Update()
    {
        UpdateNetworkStatus();
    }


    // ======================================


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


    // ======================================


    #region Photon Callbacks

    public override void OnConnectedToMaster()
    {
        regionText.text = "Region: " + PhotonNetwork.CloudRegion;
        nickNameText.text = "YOU: " + PhotonNetwork.NickName;

        EnablePanel(gameTypePanel.name);
    }

    #endregion


    // ======================================


    #region Public Callbacks

    public void __GenerateRandomName()
    {
        string pName = "p_Name_" + GetRandomNumer();
        nickNameInputText.text = pName;
    }

    public void __JoinWithNickName()
    {
        string pName = nickNameInputText.text;
        pName = (string.IsNullOrEmpty(pName)) ? "p_Name_" + GetRandomNumer() : pName;
        nickNameInputText.text = pName;

        PhotonNetwork.NickName = pName;
        PhotonNetwork.ConnectUsingSettings();

        EnablePanel(connectionPanel.name);
    }

    #endregion


    // ======================================


    #region Helpers

    private int GetRandomNumer() => Random.Range(0, 1001);

    #endregion

}
