using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsPlayerCanMove { get; set; }

    [SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private Animator playerAnimator;

    [Header("Control Settings")]
    [SerializeField] private float _mouseSensetivity = 10f;
    [SerializeField] private float _playerSpeed = 4f;

    private GameObject currentWeapon;
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
        if (currentWeapon == null)
            return;
        else
        {
            Vector3 shootDirection = transform.position + transform.forward + new Vector3(0f, 0.6f, 0f);

            Instantiate(currentWeapon, shootDirection, Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Basic"))
        {
            currentWeapon = weaponPrefabs[0];
            _gameManager.CurrentWeaponImage = currentWeapon.GetComponent<WeaponScript>().weaponImage;
        }
        else if (other.CompareTag("Grenade"))
        {
            currentWeapon = weaponPrefabs[1];
            _gameManager.CurrentWeaponImage = currentWeapon.GetComponent<WeaponScript>().weaponImage;
        }
        else if (other.CompareTag("Tennis"))
        {
            currentWeapon = weaponPrefabs[2];
            _gameManager.CurrentWeaponImage = currentWeapon.GetComponent<WeaponScript>().weaponImage;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        currentWeapon = null;
        _gameManager.SetNoWeaponImage();
    }
}
