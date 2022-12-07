using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Firing : MonoBehaviourPun
{
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform bulletFirePositionTransform;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float damageAmount = 15f;


    private Transform bulletParent;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bulletParent = GameObject.FindGameObjectWithTag("BulletParent").transform;
    }

    // animation callback
    private void _Fire()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("FireRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    private void FireRPC()
    {
        muzzleFlash.Play();
        Projectile bullet = Instantiate(projectile, bulletFirePositionTransform.position,
                            bulletFirePositionTransform.rotation, bulletParent)
                            .GetComponent<Projectile>();
        bullet.SetProjectileSpeed(projectileSpeed);
        bullet.SetStartMoving(true);

        bullet.SetDamageAmount(damageAmount);
        bullet.SetShouldInititeRPC(this.photonView.IsMine);
        bullet.SetFirePlayer(PhotonNetwork.LocalPlayer);

        audioSource.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            Debug.Break();
    }
}
