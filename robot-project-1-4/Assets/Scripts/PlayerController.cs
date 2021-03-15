using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject currentWeapon;
    public GameObject[] weaponPrefabs;
    public Animator playerAnimator;

    [Header("Control Settings")]
    public float mouseSensetivity = 100f;
    public float playerSpeed = 3f;

    private Rigidbody playerRigidbody;
    private float horizontalInput;
    private float verticalInput;
    private float playerHorizontalAngle;

    private GameManager gameManager;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerHorizontalAngle = transform.localEulerAngles.y;

    }

    void Update()
    {
        if(playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "anim_open")
        {
            MovePlayer();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            ShootFromCurrentWeapon();
        }
    }

    private void MovePlayer()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        // Move around with WASD
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput);

        moveDirection *= playerSpeed * Time.deltaTime;
        moveDirection = transform.TransformDirection(moveDirection);
        transform.position += moveDirection;

        // Turn player
        float turnPlayer = Input.GetAxis("Mouse X") * mouseSensetivity;
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

    private void ShootFromCurrentWeapon()
    {
        if(currentWeapon == null)
        {
            Debug.Log("I don't have any weapon");
        }
        else
        {
            Vector3 shootDirection = transform.position + transform.forward + new Vector3(0f, 0.6f, 0f);

            Instantiate(currentWeapon, shootDirection, Quaternion.identity);

            playerRigidbody.AddForce(-transform.forward * 2.5f, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Basic"))
        {
            currentWeapon = weaponPrefabs[0];
            gameManager.SetCurrentWeaponImage(currentWeapon.GetComponent<WeaponScript>().weaponImage);
        }
        else if (other.CompareTag("Grenade"))
        {
            currentWeapon = weaponPrefabs[1];
            gameManager.SetCurrentWeaponImage(currentWeapon.GetComponent<WeaponScript>().weaponImage);
        }
        else if (other.CompareTag("Tennis"))
        {
            currentWeapon = weaponPrefabs[2];
            gameManager.SetCurrentWeaponImage(currentWeapon.GetComponent<WeaponScript>().weaponImage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        currentWeapon = null;
        gameManager.SetNoWeaponImage();
    }
}
