using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [Header("Weapon Settings")]
    public string weaponType;
    public float weaponSpeed;
    public float destroyAfter;
    public Sprite weaponImage;
    public ParticleSystem weaponImpactFX;

    // Grenade
    public float radius = 5f;
    public float power = 10f;

    private Rigidbody weaponRigidbody;
    private Transform player;

    private float lifeCounter;
    private ParticleSystem currentWeaponImpact;

    void Start()
    {
        weaponRigidbody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        lifeCounter = 0f;

        if (weaponType == "Basic" || weaponType == "Tennis")
        {
            BehaveBasic();
        }
        else
        {
            BehaveGrenade();
        }

        transform.localEulerAngles = player.localEulerAngles;
    }

    void Update()
    {
        lifeCounter += Time.deltaTime;

        if (lifeCounter > destroyAfter)
        {
            if(currentWeaponImpact == null)
                Destroy(gameObject);
            else
            {
                Destroy(currentWeaponImpact.gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void BehaveBasic()
    {
        weaponRigidbody.AddForce(player.forward * weaponSpeed, ForceMode.Impulse);
    }

    private void BehaveGrenade()
    {
        weaponRigidbody.AddForce((player.forward + player.up) * weaponSpeed, ForceMode.Impulse);
        weaponRigidbody.AddTorque(-player.forward * 5f, ForceMode.Impulse);
    }

    private void PlayWeaponImpact()
    {
        currentWeaponImpact = Instantiate(weaponImpactFX, transform.position, Quaternion.identity);
        GetComponent<Collider>().isTrigger = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (weaponType == "Tennis")
            return;
        else if(weaponType == "Basic")
        {
            PlayWeaponImpact();
            lifeCounter = destroyAfter - 0.3f;
        }
        else
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(power * 100f, explosionPos, radius, 3f);
            }

            PlayWeaponImpact();
            lifeCounter = destroyAfter - 1f;
        }
    }
}
