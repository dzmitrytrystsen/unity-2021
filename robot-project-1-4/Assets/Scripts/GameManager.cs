using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Sprite CurrentWeaponImage
    {
        set => GameObject.Find("CurrentWeaponImage").GetComponent<Image>().sprite = value;
    }

    private PlayerController _playerController;

    [SerializeField] private Sprite _noWeaponImage;

    private void Start()
    {
        _playerController = GameObject.FindObjectOfType<PlayerController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = !Cursor.visible;
            _playerController.IsPlayerCanMove = !_playerController.IsPlayerCanMove;
        }
    }

    public void SetNoWeaponImage()
    {
        CurrentWeaponImage = _noWeaponImage;
    }

    // Restart
    public void RestartTheGame()
    {
        SceneManager.LoadScene(0);
    }
}
