using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _playerAnimator;
    private PlayerController _playerController;

    private readonly float _animationBlendSpeed = 0.1f;
    private float _targetAnimationSpeed = 0f;
    private int _comboIndex = 0;
    private float _timeForCombo = 0.4f;
    private float _timeBetweenClicks = 0.5f;
    private float _clickTime;
    private bool _ifInCombo = false;
    private int _maxComboCount = 4;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerAnimator = _playerController.GetComponent<Animator>();
    }

    private void Update()
    {
        _playerAnimator.SetFloat("speedY", _playerController.SpeedY / _playerController.PlayerJumpSpeed);

        if (Input.GetButtonDown("Jump") && !_playerController.IsJumping)
            _playerAnimator.SetTrigger("Jump");

        if (Input.GetKeyDown(KeyCode.F))
            SetDeathAnimation();

        if (Input.GetButtonDown("Fire1") && _comboIndex == 0)
        {
            _ifInCombo = true;
            _clickTime = Time.time;

            _playerAnimator.SetBool("inComboMode", true);

            EncreaseCombo();
        }
        else if (Input.GetButtonDown("Fire1") && _comboIndex > 0)
        {
            float currentClickTime = Time.time;

            if (currentClickTime - _clickTime < _timeBetweenClicks)
            {
                if(_comboIndex + 1 > _maxComboCount)
                {
                    ResetCombo();
                }
                else
                {
                    _timeForCombo += _timeBetweenClicks;
                    _clickTime = Time.time;

                    EncreaseCombo();
                }
            }
        }

        if (_ifInCombo)
        {
            _timeForCombo -= Time.deltaTime;

            if (_timeForCombo < 0)
            {
                ResetCombo();
            }
        }

        SetMoveAnimation();
    }

    private void EncreaseCombo()
    {
        _comboIndex++;
        _playerAnimator.SetInteger("comboIndex", _comboIndex);
    }

    private void ResetCombo()
    {
        _comboIndex = 0;
        _playerAnimator.SetInteger("comboIndex", _comboIndex);
        _playerAnimator.SetBool("inComboMode", false);
        _timeForCombo = _timeBetweenClicks;
        _ifInCombo = false;
    }

    private void SetMoveAnimation()
    {
        if (_playerController.MoveDirection.sqrMagnitude > 0f)
            _targetAnimationSpeed = _playerController.IsSprint ? 1f : 0.5f;
        else
            _targetAnimationSpeed = 0f;

        _playerAnimator.SetFloat("playerSpeed", Mathf.Lerp(_playerAnimator.GetFloat("playerSpeed"),
            _targetAnimationSpeed, _animationBlendSpeed));
    }

    private void SetDeathAnimation()
    {
        _playerAnimator.SetTrigger("Death");
    }
}
