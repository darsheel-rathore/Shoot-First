using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject player;

    void Start() 
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();    
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        RoomOptions roomOptions = new RoomOptions()
        {
          IsOpen = true,
          IsVisible = true,
          MaxPlayers = (byte)3  
        };
        PhotonNetwork.JoinOrCreateRoom("R_Name", roomOptions, TypedLobby.Default, expectedUsers: null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.Instantiate(player.name, RandomPosition(), Quaternion.identity);
    }

    private Vector3 RandomPosition()
    {
        Vector3 position = new Vector3(
            Random.Range(-5, 5),
            1,
            Random.Range(-5, 5)
        );

        return position;
    }

}
