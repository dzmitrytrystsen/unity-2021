using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float HorizontalInput { get { return _horizontalInput; } }
    public float VerticallInput { get { return _verticalInput; } }
    public Vector3 MoveDirection { get { return _moveDirection; } }
    public bool IsSprint { get { return Input.GetButton("Sprint"); } }
    public bool IsJumping { get { return _isJumping; } }
    public bool IsLanding { get { return _isLanding; } }
    public float SpeedY { get { return _speedY; } }
    public float PlayerJumpSpeed { get { return _playerJumpSpeed; } }

    [Header("Control Settings")]
    [SerializeField] private float _playerSpeed = 1f;
    [SerializeField] private float _playerSprintSpeed = 3f;
    [SerializeField] private float _playerJumpSpeed = 3f;
    [SerializeField] private float _rotationSpeed = 1f;

    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _moveDirection;
    private float _rotationAngle = 0f;

    private float _speedY = 0f;
    private float _gravity = -9.81f;
    private bool _isJumping;
    private bool _isLanding;
    private bool _ifCanMove;

    private CharacterController _myCharacterController;
    private Transform _myTransform;
    private Camera _myCamera;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _myCharacterController = GetComponent<CharacterController>();
        _myTransform = GetComponent<Transform>();
        _myCamera = Camera.main;
    }

    private void Update()
    {
        UseGravity();

        if (_ifCanMove)
        {
            Move();
            Rotate();
            Jump();
        }
    }

    public void SwitchCanMoveState()
    {
        _ifCanMove = !_ifCanMove;
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && !_isJumping)
        {
            _isJumping = true;
            _speedY += _playerJumpSpeed;
        }
        
        if (_isJumping && _speedY < 0f)
        {
            RaycastHit hit;
            if (Physics.Raycast(_myTransform.position, Vector3.down, out hit, 1f, LayerMask.GetMask(LayerMask.LayerToName(0))))
            {
                GetComponent<Animator>().SetTrigger("Land");
                _isJumping = false;
            }
        }
    }

    private void UseGravity()
    {
        if (!_myCharacterController.isGrounded)
            _speedY += _gravity * Time.deltaTime;
        else if (_speedY < 0f)
        {
            _speedY = 0f;
        }
    }

    private void Move()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        _moveDirection = new Vector3(_horizontalInput, 0f, _verticalInput);

        Vector3 rotatedMovement = Quaternion.Euler(0f, _myCamera.transform.rotation.eulerAngles.y, 0f) * _moveDirection;
        Vector3 verticalMovement = Vector3.up * _speedY;

        float currentSpeed = IsSprint ? _playerSprintSpeed : _playerSpeed;

        _myCharacterController.Move((verticalMovement + rotatedMovement * currentSpeed) * Time.deltaTime );

        if (rotatedMovement.sqrMagnitude > 0f)
        {
            _rotationAngle = Mathf.Atan2(rotatedMovement.x, rotatedMovement.z) * Mathf.Rad2Deg;

        }
    }

    private void Rotate()
    {
        Quaternion currentRotation = _myTransform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, _rotationAngle, 0f);

        _myTransform.rotation = Quaternion.Lerp(currentRotation, targetRotation, _rotationSpeed);
    }
}
