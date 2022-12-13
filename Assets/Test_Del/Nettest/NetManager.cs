using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject player;
    public const string DAMAGE = "damageCaused";

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
        GetComponent<CountdownTimer>().enabled = true;
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

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (changedProps.ContainsKey(PunPlayerScores.PlayerScoreProp))
            Debug.Log($"Player: {targetPlayer.ActorNumber} || Prop: {changedProps[PunPlayerScores.PlayerScoreProp]}");

        if (changedProps.ContainsKey("kill"))
            Debug.Log($"Player: {targetPlayer.ActorNumber} || Prop: {changedProps["kill"]}");

        Debug.Log("=================");

    }


    public TextMeshProUGUI timerText;
    public Image timerImage;

    public override void OnEnable()
    {
        base.OnEnable();
        CountdownTimer.OnCountdownTimerHasExpired += Expired;
    }

    private void Expired()
    {
        timerImage.enabled = true;
        Debug.Log("Timer Has finished and completed.");
        StartCoroutine(DisableImage());
    }

    private IEnumerator DisableImage()
    {
        yield return new WaitForSeconds(2f);
        timerImage.enabled = false;
    }

    public void __TestMethodBtn()
    {
        GetComponent<CountdownTimer>().enabled = true;
        Hashtable hashtable = new Hashtable();
        hashtable[CountdownTimer.CountdownStartTime] = PhotonNetwork.ServerTimestamp;
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        CountdownTimer.SetStartTime();
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
    }
}
