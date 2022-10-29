using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firing : MonoBehaviour
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
        muzzleFlash.Play();
        Projectile bullet = Instantiate(projectile, bulletFirePositionTransform.position,
                            bulletFirePositionTransform.rotation, bulletParent)
                            .GetComponent<Projectile>();
        bullet.SetProjectileSpeed(projectileSpeed);
        bullet.SetStartMoving(true);
        bullet.SetDamageAmount(damageAmount);
        
        audioSource.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            Debug.Break();
    }
}
