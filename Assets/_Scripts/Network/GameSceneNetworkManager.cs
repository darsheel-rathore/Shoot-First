using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameSceneNetworkManager : MonoBehaviourPunCallbacks
{

    [SerializeField] Transform spawnpointList;
    [SerializeField] GameObject playerPrefab;

    [SerializeField] Transform scorecardParent;
    [SerializeField] GameObject scorecardPrefab;
    [SerializeField] GameObject scoreBoard;
    bool scoreBoardOpen = false;

    public Dictionary<Player, GameObject> scorecardDict = new Dictionary<Player, GameObject>();

    private void Start() 
    {
        PhotonNetwork.Instantiate(playerPrefab.name, spawnpointList.GetChild(GetSpawnIndex()).position, Quaternion.identity);

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            InstantiateAndAddScorecard(player);
        }
    }

    private int GetSpawnIndex()
    {
        var playerList = PhotonNetwork.PlayerList;
        return Array.IndexOf(playerList, PhotonNetwork.LocalPlayer);
    }


    public void _ScoreBoardToggleBtn()
    {
        scoreBoardOpen = !scoreBoardOpen;
        scoreBoard.SetActive(scoreBoardOpen);
    }

    #region Photon Callbacks

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        //DebugCode(targetPlayer, changedProps);

        GameObject scorecardToUpdate = scorecardDict[targetPlayer];
        UpdateScoreboardProperties(scorecardToUpdate, changedProps);
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        InstantiateAndAddScorecard(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        RemovePlayer(otherPlayer);
    }


    #endregion


    #region Private Methods

    private void InstantiateAndAddScorecard(Player playerToAdd)
    {
        GameObject scorecard = Instantiate(scorecardPrefab, scorecardParent);
        scorecardDict.Add(playerToAdd, scorecard);
        scorecard.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerToAdd.NickName;

        UpdateScoreboardProperties(scorecard, playerToAdd.CustomProperties);
    }

    private void UpdateScoreboardProperties(GameObject scorecard, ExitGames.Client.Photon.Hashtable playerProps)
    {
        TextMeshProUGUI[] propTexts = new TextMeshProUGUI[2];
        for (int i = 0; i < 2; i++)
            propTexts[i] = scorecard.transform.GetChild(i + 1).GetComponent<TextMeshProUGUI>();


        if (playerProps.ContainsKey("playerKills"))
            propTexts[0].text = playerProps["playerKills"].ToString();

        if (playerProps.ContainsKey(PunPlayerScores.PlayerScoreProp))
            propTexts[1].text = playerProps[PunPlayerScores.PlayerScoreProp].ToString();
    }

    private void RemovePlayer(Player playerToRemove)
    {
        GameObject scorecardToRemove = scorecardDict[playerToRemove];
        scorecardDict.Remove(playerToRemove);
        Destroy(scorecardToRemove);
    }

    #endregion


    #region Debug Code

    //private static void DebugCode(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    //{
    //    foreach (var item in changedProps.Keys)
    //    {
    //        Debug.Log("KEYS: -- Player: " + targetPlayer.ActorNumber + " || " + item.ToString());
    //    }

    //    if (targetPlayer.CustomProperties.ContainsKey("playerKills"))
    //        Debug.Log($"Player: {targetPlayer.ActorNumber} || Kills: {changedProps["playerKills"]}");

    //    if (targetPlayer.CustomProperties.ContainsKey(PunPlayerScores.PlayerScoreProp))
    //        Debug.Log($"Player: {targetPlayer.ActorNumber} || Score: {changedProps[PunPlayerScores.PlayerScoreProp]}");

    //    Debug.Log("=========");
    //}


    #endregion
}
