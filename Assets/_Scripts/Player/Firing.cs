using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firing : MonoBehaviour
{
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject projectile;

    // animation callback
    private void _Fire()
    {
        muzzleFlash.Play();
    }
}
