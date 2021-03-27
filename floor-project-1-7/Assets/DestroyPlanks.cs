using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlanks : MonoBehaviour
{
    [SerializeField] private Rigidbody[] planks;

    private void Start()
    {
        planks = GetComponentsInChildren<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            foreach (Rigidbody plank in planks)
            {
                plank.AddForce(-transform.up * 1.5f, ForceMode.Impulse);
                plank.gameObject.GetComponent<BoxCollider>().enabled = false;
            }

            StartCoroutine(SwitchKinematicAfterSecs());
        }
    }

    IEnumerator SwitchKinematicAfterSecs()
    {
        yield return new WaitForSeconds(0.2f);

        foreach (Rigidbody plank in planks)
        {
            plank.isKinematic = false;
            plank.gameObject.GetComponent<Collider>().enabled = true;
        }

        StopAllCoroutines();
    }
}
