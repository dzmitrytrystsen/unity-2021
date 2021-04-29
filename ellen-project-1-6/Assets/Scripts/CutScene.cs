using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutScene : MonoBehaviour
{
    [SerializeField] private PlayableDirector _director;

    [SerializeField] private GameObject _playerCutScene;
    [SerializeField] private GameObject _timelineCameras;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _freeLookCamera;

    private void Update()
    {
        if (_director.state == PlayState.Paused)
        {
            Destroy(_timelineCameras);
            Destroy(_playerCutScene);
            _player.SetActive(true);
            _freeLookCamera.SetActive(true);
            Destroy(gameObject);
        }
            
    }
}
