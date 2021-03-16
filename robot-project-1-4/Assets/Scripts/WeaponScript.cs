using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [Header("Weapon Settings")]
    public Sprite weaponImage;
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private float _weaponSpeed;
    [SerializeField] private float _destroyAfter;
    [SerializeField] private ParticleSystem _weaponImpactFX;

    private enum WeaponType { Basic, Grenade, Tennis };

    // Grenade
    private float _radius = 5f;
    private float _power = 10f;

    private Rigidbody _weaponRigidbody;
    private Transform _player;

    private float _lifeCounter;
    private ParticleSystem _currentWeaponImpact;

    private void Start()
    {
        _weaponRigidbody = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        _lifeCounter = 0f;

        if (_weaponType == WeaponType.Basic || _weaponType == WeaponType.Tennis)
        {
            BehaveBasic();
        }
        else
        {
            BehaveGrenade();
        }

        transform.localEulerAngles = _player.localEulerAngles;
    }

    private void Update()
    {
        _lifeCounter += Time.deltaTime;

        if (_lifeCounter > _destroyAfter)
        {
            if(_currentWeaponImpact == null)
                Destroy(gameObject);
            else
            {
                Destroy(_currentWeaponImpact.gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void BehaveBasic()
    {
        _weaponRigidbody.AddForce(_player.forward * _weaponSpeed, ForceMode.Impulse);
    }

    private void BehaveGrenade()
    {
        _weaponRigidbody.AddForce((_player.forward + _player.up) * _weaponSpeed, ForceMode.Impulse);
        _weaponRigidbody.AddTorque(-_player.forward * 5f, ForceMode.Impulse);
    }

    private void PlayWeaponImpact()
    {
        _currentWeaponImpact = Instantiate(_weaponImpactFX, transform.position, Quaternion.identity);
        GetComponent<Collider>().isTrigger = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_weaponType == WeaponType.Tennis)
            return;
        else if(_weaponType == WeaponType.Basic)
        {
            PlayWeaponImpact();
            _lifeCounter = _destroyAfter - 0.3f;
        }
        else
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, _radius);
            foreach (Collider hit in colliders)
            {
                if (!hit.CompareTag("Player"))
                {
                    Rigidbody rb = hit.GetComponent<Rigidbody>();

                    if (rb != null)
                        rb.AddExplosionForce(_power * 100f, explosionPos, _radius, 3f);
                }
            }

            PlayWeaponImpact();
            _lifeCounter = _destroyAfter - 1f;
        }
    }
}
