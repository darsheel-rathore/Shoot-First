using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{

    Rigidbody rb;
    [SerializeField]
    Transform projectile, projectileTransform;

    [SerializeField] private float health = 100;

    [SerializeField] private int damageDone;
    [SerializeField] private int kills;

    public int playerScore = 0;
    Player thisPlayer;

    ExitGames.Client.Photon.Hashtable killed = new ExitGames.Client.Photon.Hashtable();

    void Start()
    {
        if (photonView.IsMine)
        {
            killed["kill"] = 0;
            PhotonNetwork.LocalPlayer.SetCustomProperties(killed);
        }

        rb = GetComponent<Rigidbody>();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (this.photonView.OwnerActorNr == player.ActorNumber)
            {
                thisPlayer = player;
                break;
            }
        }
    }

    private void Update()
    {

        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC("ShootSphere", RpcTarget.All, thisPlayer);
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
            CallbackExecutionTimeTest(PhotonNetwork.LocalPlayer);

    }


    [PunRPC]
    private void ShootSphere(Player player)
    {
        Transform t = Instantiate(projectile, projectileTransform.position, projectileTransform.rotation);
        t.GetComponent<Proj>().SetFireFromPlayer(thisPlayer);
        t.GetComponent<Proj>().SetInitiateRPC(photonView.IsMine);
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        float inH = Input.GetAxis("Horizontal");
        float inV = Input.GetAxis("Vertical");

        Vector3 moveVector = new Vector3(inH, 0, inV);
        moveVector.Normalize();

        rb.MovePosition(transform.position + moveVector * 0.2f);

        if (moveVector.magnitude <= 0.5f) return;

        Quaternion lookRotation = Quaternion.LookRotation(moveVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 5);
    }

    public void TakeSphereDamage(float damageAmount, Player firePlayer)
    {
        this.photonView.RPC("_DealDamageWithSphereRPC", RpcTarget.All, damageAmount, firePlayer);
    }


    [PunRPC]
    public void _DealDamageWithSphereRPC(float damageAmount, Player playerFire)
    {
        if (photonView.IsMine)
        {
            playerFire.AddScore(10);
        }
        health -= damageAmount;
        if (health <= 50) DieMethod(playerFire);
    }

    bool isDead = false;
    private void DieMethod(Player playerFire)
    {
        if (isDead) return;

        if (photonView.IsMine)
        {
            UpdateKillProp(playerFire);
        }

        Debug.Log("DEAD");

    }

    private void CallbackExecutionTimeTest(Player player)
    {
        UpdateKillProp(player);
        player.AddScore(10);
    }

    private void UpdateKillProp(Player playerFire)
    {
        var updateProp = playerFire.CustomProperties;
        int countKill = (int)updateProp["kill"];
        //int countScore = (int)updateProp[PunPlayerScores.PlayerScoreProp] + 10;
        playerFire.AddScore(10);

        countKill++;
        updateProp["kill"] = countKill;
        //updateProp[PunPlayerScores.PlayerScoreProp] = countScore;
        playerFire.SetCustomProperties(updateProp);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.health);
        }
        else if (stream.IsReading)
        {
            this.health = (float)stream.ReceiveNext();
        }
    }

    public float GetHealth() => health;


}