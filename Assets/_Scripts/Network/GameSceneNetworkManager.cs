using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameSceneNetworkManager : MonoBehaviour
{
    [SerializeField] Transform spawnpointList;
    [SerializeField] GameObject playerPrefab;

    private void Start() 
    {
        PhotonNetwork.Instantiate(playerPrefab.name, spawnpointList.GetChild(GetSpawnIndex()).position, Quaternion.identity);
    }

    private int GetSpawnIndex()
    {
        var playerList = PhotonNetwork.PlayerList;
        return Array.IndexOf(playerList, PhotonNetwork.LocalPlayer);
    }
}
