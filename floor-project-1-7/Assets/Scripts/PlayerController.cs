using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Control Settings")]
    [SerializeField] private float _playerWalkSpeed = 2f;
    [SerializeField] private float _playerSprintSpeed = 3.5f;
    [SerializeField] private float _mouseSensitivityX = 100f;
    [SerializeField] private float _mouseSensitivityY = 50f;

    private float _gravityValue = -9.81f;
    private float _verticalSpeed = 0;
    private float _horizontalAngle, _verticalAngle;

    private bool _isSprint { get { return Input.GetButton("Sprint"); } }

    private CharacterController _characterController;
    private Camera _mainCamera;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _characterController = GetComponent<CharacterController>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        Move();
        Rotate();
        UseGravity();
    }


    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, _verticalSpeed, verticalInput);

        float currentSpeed = _isSprint ? _playerSprintSpeed : _playerWalkSpeed;

        moveDirection = moveDirection * currentSpeed * Time.deltaTime;
        moveDirection = transform.TransformDirection(moveDirection);

        _characterController.Move(moveDirection);
    }

    private void Rotate()
    {
        // Rotate by Y
        float turnDirection = Input.GetAxis("Mouse X") * _mouseSensitivityX * Time.deltaTime;

        _horizontalAngle += turnDirection;

        if (_horizontalAngle > 360)
            _horizontalAngle -= 360.0f;

        if (_horizontalAngle < 0)
            _horizontalAngle += 360.0f;

        Vector3 currentAngles = transform.localEulerAngles;
        currentAngles.y = _horizontalAngle;
        transform.localEulerAngles = currentAngles;

        // Camera look up/down
        float turnCam = -Input.GetAxis("Mouse Y") * _mouseSensitivityY * Time.deltaTime;
        _verticalAngle = Mathf.Clamp(turnCam + _verticalAngle, -89.0f, 69.0f);
        currentAngles = _mainCamera.transform.localEulerAngles;
        currentAngles.x = _verticalAngle;
        _mainCamera.transform.localEulerAngles = currentAngles;
    }

    private void UseGravity()
    {
        if (!_characterController.isGrounded)
            _verticalSpeed += _gravityValue * Time.deltaTime;
        else if (_verticalSpeed < 0f)
        {
            _verticalSpeed = 0f;
        }
    }
}
