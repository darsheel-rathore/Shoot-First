using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float projectileSpeed = 5f;
    private bool startMoving = false;

    private Vector3 forwardDirection;
    private float damageAmount;

    private void Update()
    {
        if (!startMoving) return;

        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.transform.CompareTag("Player"))
        {
            PhotonView pv = other.GetComponent<PhotonView>();
            Health health = other.GetComponent<Health>();
            pv.RPC("TakeDamage", RpcTarget.AllBuffered, damageAmount);
        }
    }

    public void SetProjectileSpeed(float speed) => projectileSpeed = speed;
    public void SetStartMoving(bool isMoving) => startMoving = isMoving;
    public void SetDamageAmount(float damageAmount) => this.damageAmount = damageAmount;
}