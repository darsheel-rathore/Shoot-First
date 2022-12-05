using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Proj : MonoBehaviour
{
    Player firePlayer;
    private bool shouldInitiateRPC = false;
    public int damageAmount = 10;

    private void Start()
    {
        Invoke("DestroyTimer", 1f);
    }

    private void DestroyTimer()
    {
        Destroy(this.gameObject, 5f);
    }

    private void Update()
    {
        transform.position += transform.forward * 10f * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            if (shouldInitiateRPC)
            {
                other.GetComponent<PlayerMove>().TakeSphereDamage(damageAmount, firePlayer);
            }
            Destroy(this.gameObject);
        }
    }

    public void SetInitiateRPC(bool value) => shouldInitiateRPC = value;
    public void SetFireFromPlayer(Player fromPlayer) => firePlayer = fromPlayer;

}
