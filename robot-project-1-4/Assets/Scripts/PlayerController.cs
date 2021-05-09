using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void ShootAction();
    public static event ShootAction OnShot;

    public bool IsPlayerCanMove { get; set; }

    [SerializeField] private Animator playerAnimator;

    [Header("Control Settings")]
    [SerializeField] private float _mouseSensetivity = 10f;
    [SerializeField] private float _playerSpeed = 4f;

    private enum WeaponType
    {
        Pistol,
        Grenade,
        Ball,
        Empty
    }

    private WeaponType currentWeapon;
    private float horizontalInput;
    private float verticalInput;
    private float playerHorizontalAngle;
    private GameManager _gameManager;
    private bool IsPlayerInOpenAnimation { get => playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "anim_open"; }
    

    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerHorizontalAngle = transform.localEulerAngles.y;
        IsPlayerCanMove = true;

        currentWeapon = WeaponType.Empty;
    }

    void Update()
    {
        MovePlayer();

        if (Input.GetButtonDown("Fire1"))
        {
            ShootFromCurrentWeapon();
        }
    }

    private void MovePlayer()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        if (IsPlayerCanMove && IsPlayerInOpenAnimation)
        {
            // Move around with WASD
            Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput);

            moveDirection *= _playerSpeed * Time.deltaTime;
            moveDirection = transform.TransformDirection(moveDirection);
            transform.position += moveDirection;

            // Turn player
            float turnPlayer = Input.GetAxis("Mouse X") * _mouseSensetivity;
            playerHorizontalAngle += turnPlayer;

            Vector3 currentAngles = transform.localEulerAngles;
            currentAngles.y = playerHorizontalAngle;
            transform.localEulerAngles = currentAngles;

            // Animate Player
            if (moveDirection.sqrMagnitude > 0f || Mathf.Abs(Input.GetAxis("Mouse X")) > 0f)
            {
                playerAnimator.SetBool("Walk_Anim", true);
            }
            else
            {
                playerAnimator.SetBool("Walk_Anim", false);
            }
        }
    }

    private void ShootFromCurrentWeapon()
    {
        if (currentWeapon == WeaponType.Empty)
            return;
        else
        {
            Vector3 shootDirection = transform.position + transform.forward + new Vector3(0f, 0.6f, 0f);

            if (currentWeapon == WeaponType.Pistol)
            {
                GameObject currentProjectile = ProjectileManager.Instance.GetPooledProjectile(ProjectileManager.WeaponType.Pistol);
                currentProjectile.transform.position = shootDirection;
                currentProjectile.SetActive(true);
                OnShot?.Invoke();
            }
            else if (currentWeapon == WeaponType.Grenade)
            {
                GameObject currentProjectile = ProjectileManager.Instance.GetPooledProjectile(ProjectileManager.WeaponType.Grenade);
                currentProjectile.transform.position = shootDirection;
                currentProjectile.SetActive(true);
                OnShot?.Invoke();
            }
            else
            {
                GameObject currentProjectile = ProjectileManager.Instance.GetPooledProjectile(ProjectileManager.WeaponType.Ball);
                currentProjectile.transform.position = shootDirection;
                currentProjectile.SetActive(true);
                OnShot?.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Basic"))
        {
            currentWeapon = WeaponType.Pistol;
            _gameManager.CurrentWeaponImage = ProjectileManager.Instance.PistolProjectilePrefab.GetComponent<WeaponScript>().weaponImage;
        }
        else if (other.CompareTag("Grenade"))
        {
            currentWeapon = WeaponType.Grenade;
            _gameManager.CurrentWeaponImage = ProjectileManager.Instance.GrenadeProjectilePrefab.GetComponent<WeaponScript>().weaponImage;
        }
        else if (other.CompareTag("Tennis"))
        {
            currentWeapon = WeaponType.Ball;
            _gameManager.CurrentWeaponImage = ProjectileManager.Instance.BallProjectilePrefab.GetComponent<WeaponScript>().weaponImage;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        currentWeapon = WeaponType.Empty;
        _gameManager.SetNoWeaponImage();
    }
}
