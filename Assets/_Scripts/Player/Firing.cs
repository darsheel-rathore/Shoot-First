using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firing : MonoBehaviour
{
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform bulletFirePositionTransform;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private float damageAmount = 15f;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
