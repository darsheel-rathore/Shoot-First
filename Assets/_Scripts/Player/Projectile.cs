using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float timeBeforeDeactivation;

    private float projectileSpeed = 5f;
    private bool startMoving = false;

    private Vector3 forwardDirection;
    private int damageAmount;

    private bool shouldInitiateRPC = false;
    private Player playerWhoFired;



    private void Start()
    {
        Invoke("Deactivate", timeBeforeDeactivation);
    }

    private void Update()
    {
        if (!startMoving) return;

        transform.position += projectileSpeed * Time.deltaTime * transform.forward;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.transform.CompareTag("Player"))
        {
            if (shouldInitiateRPC)
            {
                other.GetComponent<Health>().TakeBulletDamage(damageAmount, playerWhoFired);
            }
        }
        this.gameObject.SetActive(false);
    }

    private void Deactivate() => this.gameObject.SetActive(false);

    public void SetProjectileSpeed(float speed) => projectileSpeed = speed;
    public void SetStartMoving(bool isMoving) => startMoving = isMoving;
    public void SetDamageAmount(int damageAmount) => this.damageAmount = damageAmount;
    public void SetShouldInititeRPC(bool initiateRPC) => shouldInitiateRPC = initiateRPC;
    public void SetFirePlayer(Player firingPlayer) => playerWhoFired = firingPlayer;
}