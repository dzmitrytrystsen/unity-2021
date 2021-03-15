using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject currentWeaponImage;
    public Sprite noWeaponImage;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void SetCurrentWeaponImage(Sprite weaponImage)
    {
        currentWeaponImage.GetComponent<Image>().sprite = weaponImage;
    }

    public void SetNoWeaponImage()
    {
        currentWeaponImage.GetComponent<Image>().sprite = noWeaponImage;
    }

    // Restart
    public void RestartTheGame()
    {
        SceneManager.LoadScene(0);
    }
}
