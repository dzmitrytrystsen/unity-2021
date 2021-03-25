using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsLooper : MonoBehaviour
{
    [SerializeField] private GameObject _floorBlockPrefab;

    private Vector3 nextFloorPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            nextFloorPosition = transform.parent.position + new Vector3(0f, 8f, 0f);
            Instantiate(_floorBlockPrefab, nextFloorPosition, Quaternion.identity);
        }
    }
}
