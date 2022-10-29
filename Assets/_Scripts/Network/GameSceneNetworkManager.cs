using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameSceneNetworkManager : MonoBehaviour
{
    [SerializeField] Transform spawnpointList;
    [SerializeField] GameObject playerPrefab;

    private void Start() 
    {
        PhotonNetwork.Instantiate(playerPrefab.name, spawnpointList.GetChild(GetRandomIndex()).position, Quaternion.identity);
    }

    private int GetRandomIndex() => Random.Range(0, spawnpointList.childCount);
}
