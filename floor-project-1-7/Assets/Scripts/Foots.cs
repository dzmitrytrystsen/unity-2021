using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foots : MonoBehaviour
{
    public bool IsWalk { get; set; }
    public bool IsRun { get; set; }

    public enum MoveType { Walk, Sprint, Idle };
    public MoveType CurrentMoveType { get; set; }

    [Header("Audio Settings")]
    [SerializeField] private AudioClip _walkSound;
    [SerializeField] private AudioClip _runSound;

    private AudioSource _audioSource;
    private bool _isSoundSwitch = false;
    private MoveType _imInMoveType = MoveType.Idle;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentMoveType == MoveType.Walk)
        {
            SetMoveSound(_walkSound);
        }
        else if (CurrentMoveType == MoveType.Sprint)
        {
            SetMoveSound(_runSound);
        }
        else
        {
            _audioSource.clip = null;
            _audioSource.Stop();
            _isSoundSwitch = false;
        }
    }

    private void SetMoveSound(AudioClip audioClip)
    {
        if (!_isSoundSwitch || _imInMoveType != CurrentMoveType)
        {
            _imInMoveType = CurrentMoveType;

            _audioSource.clip = audioClip;
            _audioSource.Play();
            _isSoundSwitch = true;
        }
    }
}
