using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameSceneNetworkManager : MonoBehaviourPunCallbacks
{
    public static GameSceneNetworkManager Instance;

    [SerializeField] Transform spawnpointList;
    [SerializeField] GameObject playerPrefab;

    [SerializeField] Transform scorecardParent;
    [SerializeField] GameObject scorecardPrefab;
    [SerializeField] GameObject scoreBoard;
    private bool scoreBoardOpen = false;

    public Dictionary<Player, GameObject> scorecardDict = new Dictionary<Player, GameObject>();

    [SerializeField] GameObject leaveGamePanel;
    [SerializeField] GameObject gameoverPanel;
    [SerializeField] GameObject confirmLeaveGamePanel;
    [SerializeField] TextMeshProUGUI killTxt, damageTxt;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject scoreBoardBtn, leaveGameOptnBtn;

    [SerializeField] CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GameObject g = PhotonNetwork.Instantiate(playerPrefab.name, spawnpointList.GetChild(GetSpawnIndex()).position, Quaternion.identity);
        virtualCamera.Follow = g.transform;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            InstantiateAndAddScorecard(player);
        }
    }

    private int GetSpawnIndex()
    {
        int position = PhotonNetwork.LocalPlayer.ActorNumber;
        int numberOfSpawnPoints = spawnpointList.transform.childCount;

        position = (position % numberOfSpawnPoints == 0) ? (numberOfSpawnPoints - 1) : (position % numberOfSpawnPoints - 1);

        return position;
    }


    public void GameOver()
    {
        leaveGameOptnBtn.SetActive(false);
        scoreBoardBtn.SetActive(false);

        gameCanvas.SetActive(false);
        gameoverPanel.SetActive(true);
        Player p = PhotonNetwork.LocalPlayer;

        if (p.CustomProperties.ContainsKey("playerKills"))
            killTxt.text = "KILLS - " + p.CustomProperties["playerKills"].ToString();

        if (p.CustomProperties.ContainsKey(PunPlayerScores.PlayerScoreProp))
            damageTxt.text = "DAMAGE - " + p.CustomProperties[PunPlayerScores.PlayerScoreProp].ToString();

        StartCoroutine(LeaveGameRouting());
    }

    private IEnumerator LeaveGameRouting()
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.LeaveRoom();
    }


    #region Public Callbacks

    public void _ScoreBoardToggleBtn()
    {
        scoreBoardOpen = !scoreBoardOpen;
        scoreBoard.SetActive(scoreBoardOpen);
        gameCanvas.SetActive(!scoreBoardOpen);
        leaveGameOptnBtn.SetActive(!scoreBoardOpen);
    }


    public void _LeaveGameOptionBtn()
    {
        gameCanvas.SetActive(false);
        leaveGamePanel.SetActive(true);
        scoreBoardBtn.SetActive(false);
    }


    public void _LeaveGameConfirmBtn()
    {
        leaveGamePanel.SetActive(false);
        confirmLeaveGamePanel.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }


    public void _CancelLeaveGameBtn()
    {
        gameCanvas.SetActive(true);
        leaveGamePanel.SetActive(false);
        scoreBoardBtn.SetActive(true);
    }


    #endregion


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

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        PhotonNetwork.LoadLevel(0);
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
