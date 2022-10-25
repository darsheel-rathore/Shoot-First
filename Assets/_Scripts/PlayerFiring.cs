using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFiring : MonoBehaviour
{

    [SerializeField] private Transform flashPositionTransform;
    [SerializeField] private Transform firePositionTransform;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem projectile;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Fire()
    {
        muzzleFlash.Play();
        Instantiate(projectile.gameObject, flashPositionTransform.position + flashPositionTransform.forward, firePositionTransform.rotation);
        audioSource.Play();
    }

}
