using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Health : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Color fullHealthColor;
    [SerializeField] private Color zeroHealthColor;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;

    private float currentHealth = 100;
    private int damageAmount = 0;
    private bool isDead = false;

    Hashtable playerKillCount = new Hashtable();


    private void Start()
    {
        if (this.photonView.IsMine)
        {
            playerKillCount["playerKills"] = 0;
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerKillCount);
        }
        //RestoreHealth();
        SetHealthUI();
    }

    [PunRPC]
    public void TakeDamage(int damageAmount, Player playerFired)
    {
        UpdateHealthRPC(damageAmount, playerFired);
    }

    private void UpdateHealthRPC(int damageAmount, Player playerFired)
    {
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0f);

        SetHealthUI();

        if (currentHealth <= 0) Die(playerFired);
    }

    internal void TakeBulletDamage(int damageAmount, Player playerWhoFired)
    {
        this.damageAmount = damageAmount;
        this.photonView.RPC("TakeDamage", RpcTarget.AllViaServer, damageAmount, playerWhoFired);
        UpdateDamageProperty(damageAmount, playerWhoFired);
    }

    private static void UpdateDamageProperty(int damageAmount, Player playerWhoFired)
    {
        playerWhoFired.AddScore((int)damageAmount);
    }

    public void SetHealthUI()
    {
        healthSlider.value = currentHealth;

        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealth / maxHealth);
    }

    private void Die(Player playerFired)
    {
        if (isDead) return;

        if (photonView.IsMine)
        {
            UpdateKillProperty(playerFired);
        }


        Debug.Log("Dead -" + this.gameObject);
        gameObject.SetActive(false);
        isDead = true;
    }

    private void UpdateKillProperty(Player playerFired)
    {
        var updateProp = playerFired.CustomProperties;
        int killCount = (int)updateProp["playerKills"];
        int scoreCount = (int)updateProp[PunPlayerScores.PlayerScoreProp];

        scoreCount += 15;
        killCount += 1;

        updateProp["playerKills"] = killCount;
        updateProp[PunPlayerScores.PlayerScoreProp] = scoreCount;
        
        // not working find Why???
        //Debug.Log(x);
        //x += damageAmount;
        //Debug.Log(x);

        playerFired.SetCustomProperties(updateProp);
    }

    private void RestoreHealth() => currentHealth = maxHealth;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.currentHealth);
        }
        else if (stream.IsReading)
        {
            this.currentHealth = (float)stream.ReceiveNext();
        }
    }
}
