using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDestroyer : MonoBehaviour
{
    [SerializeField] GameObject[] levelObjects;

    private void OnTriggerEnter(Collider other)
    {
        foreach (GameObject levelObject in levelObjects)
        {
            Destroy(levelObject);
        }
    }
}
