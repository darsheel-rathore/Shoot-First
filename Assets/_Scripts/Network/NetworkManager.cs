using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Panels

    [SerializeField] private GameObject nickNamePanel;
    [SerializeField] private GameObject connectionPanel;
    [SerializeField] private GameObject gameTypePanel;
    [SerializeField] private GameObject joinCreateRoomPanel;
    [SerializeField] private GameObject joiningRoomPanel;
    [SerializeField] private GameObject creatingRoomPanel;
    [SerializeField] private GameObject createdRoomPanel;
    [SerializeField] private GameObject joinedRoomPanel;
    [SerializeField] private GameObject joinRoomFailedPanel;

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
        joiningRoomPanel.SetActive(joiningRoomPanel.name == panelName);
        creatingRoomPanel.SetActive(creatingRoomPanel.name == panelName);
        createdRoomPanel.SetActive(createdRoomPanel.name == panelName);
        joinedRoomPanel.SetActive(joinedRoomPanel.name == panelName);
        joinRoomFailedPanel.SetActive(joinRoomFailedPanel.name == panelName);
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


    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        EnablePanel(createdRoomPanel.name); 
        Debug.Log("test_created");
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Creating room failed... Retrying");
        __CreateRoom();
    }


    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        EnablePanel(joinRoomFailedPanel.name);
    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        EnablePanel(joinedRoomPanel.name);
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


    public void __FreeForAll() => EnablePanel(joinCreateRoomPanel.name);


    public void __CreateRoom()
    {
        string rName = "r_Name_" + GetRandomNumer();
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = (byte)5
        };

        PhotonNetwork.CreateRoom(rName, roomOptions);
        EnablePanel(creatingRoomPanel.name);
    }


    public void __JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
        EnablePanel(joiningRoomPanel.name);
    }


    public void __BackButtonForJoinFailed()
    {
        EnablePanel(joinCreateRoomPanel.name);
    }


    #endregion


    // ======================================


    #region Helpers

    private int GetRandomNumer() => Random.Range(0, 1001);

    #endregion

}
