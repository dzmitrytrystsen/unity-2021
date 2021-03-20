using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool IsGrounded => _isGrounded;
    private float _turnDirectionX = 0f;
    private float _turnDirectionY = 0f;
    public CharacterController MyCharacterController { get { return myCharacterController = myCharacterController ?? GetComponent<CharacterController>(); } }

    [Header("Control Settings")]
    [SerializeField] private float _jumpSpeed = 5f;
    [SerializeField] private float _playerSpeed = 3f;
    [SerializeField] private float _touchSensetivityX = 0.1f;
    [SerializeField] private float _touchSensetivityY = 0.05f;

    [SerializeField] private Joystick _myJoystick;

    private float _gravityValue = 9.81f;
    private float _verticalSpeed = 0f;
    private Vector3 _moveDirection;
    private float _verticalAngle, _horizontalAngle;
    private bool _isGrounded;
    private bool _loosedGrounding;
    private float _groundedTimer;
    private CharacterController myCharacterController;
    private Camera _mainCamera;
    private Transform _myTransform;
    private Vector3 _hitNormal;
    private float _slopeLimit;

    private void Start()
    {
        _myTransform = GetComponent<Transform>();
        _mainCamera = Camera.main;
        _slopeLimit = MyCharacterController.slopeLimit;
        _isGrounded = true;
        _loosedGrounding = false;
    }

    private void Update()
    {
        if (!MyCharacterController.isGrounded)
        {
            if (_isGrounded)
            {
                _groundedTimer += Time.deltaTime;

                if (_groundedTimer >= 0.1f)
                {
                    _loosedGrounding = true;
                    _isGrounded = false;
                }
            }
        }
        else
        {
            _groundedTimer = 0.0f;
            _isGrounded = true;
            _loosedGrounding = false;
        }

        UseGravity();
        Move();
        Turn(_turnDirectionX, _turnDirectionY);

        if(!_loosedGrounding)
            _isGrounded = (Vector3.Angle(Vector3.up, _hitNormal)) <= _slopeLimit;
    }

    public void Move()
    {
        _moveDirection = new Vector3(_myJoystick.Horizontal, -1f, _myJoystick.Vertical);

        _moveDirection = _moveDirection * _playerSpeed * Time.deltaTime;
        _moveDirection = _myTransform.TransformDirection(_moveDirection);

        MyCharacterController.Move(_moveDirection);
    }

    public void Turn(float turnDirectionX, float turnDirectionY)
    {
        _horizontalAngle += turnDirectionX * _touchSensetivityX;

        if (_horizontalAngle > 360) _horizontalAngle -= 360f;
        if (_horizontalAngle < 0) _horizontalAngle += 360f;

        Vector3 currentAngles = _myTransform.localEulerAngles;
        currentAngles.y = _horizontalAngle;
        _myTransform.localEulerAngles = currentAngles;

        float turnY = -turnDirectionY * _touchSensetivityY;
        _verticalAngle = Mathf.Clamp(turnY + _verticalAngle, -5f, 25f);
        currentAngles = _mainCamera.transform.localEulerAngles;
        currentAngles.x = _verticalAngle;
        _mainCamera.transform.localEulerAngles = currentAngles;
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            _verticalSpeed = _jumpSpeed;
            _isGrounded = false;
            _loosedGrounding = true;
        }

    }

    private void UseGravity()
    {
        _verticalSpeed -= _gravityValue * Time.deltaTime;

        if (_verticalSpeed < -_gravityValue)
            _verticalSpeed = -_gravityValue;

        var verticalMove = Vector3.up * _verticalSpeed * Time.deltaTime;
        var flag = MyCharacterController.Move(verticalMove);

        if ((flag & CollisionFlags.Below) != 0)
            _verticalSpeed = 0;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _hitNormal = hit.normal;
    }
}
