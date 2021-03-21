using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Control Settings")]
    [SerializeField] private float _playerSpeed = 1f;

    private float _horizontalInput;
    private float _verticalInput;

    private CharacterController _myCharacterController;
    private Transform _myTransform;

    private void Start()
    {
        _myCharacterController = GetComponent<CharacterController>();
        _myTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        Move();
    }


    private void Move()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(_horizontalInput, 0f, _verticalInput) * _playerSpeed * Time.deltaTime;
        moveDirection = _myTransform.TransformDirection(moveDirection);

        _myCharacterController.Move(moveDirection);
    }
}
