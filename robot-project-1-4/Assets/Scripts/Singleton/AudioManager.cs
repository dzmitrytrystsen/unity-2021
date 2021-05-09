using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioClip pistolFireSound;
    [SerializeField] private AudioClip bulletImpact;

    private AudioSource _audioSource;

    private void Start()
    {
        PlayerController.OnShot += PlayFireSound;
        WeaponScript.OnProjectileCollision += PlayCollisionSound;

        _audioSource = GetComponent<AudioSource>();
    }

    private void PlayFireSound()
    {
        _audioSource.PlayOneShot(pistolFireSound);
    }

    private void PlayCollisionSound()
    {
        _audioSource.PlayOneShot(bulletImpact);
    }
}
