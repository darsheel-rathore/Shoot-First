using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameSceneNetworkManager : MonoBehaviourPunCallbacks
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

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        Debug.Log($"Name: {targetPlayer} Score: {changedProps}");
    }
}
