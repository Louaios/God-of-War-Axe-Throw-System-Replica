using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeCollisionHandler : MonoBehaviour
{
    private bool hasHit;
    private Collider axeCollider;

    private void Start()
    {
        axeCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;

        BreakBoxScript breakBox = collision.gameObject.GetComponent<BreakBoxScript>();
        if (breakBox != null)
        {
            hasHit = true;
            breakBox.Break();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (hasHit) return;

        BreakBoxScript breakBox = collision.gameObject.GetComponent<BreakBoxScript>();
        if (breakBox != null)
        {
            hasHit = true;
            breakBox.Break();
        }
    }

    public void ResetHit()
    {
        hasHit = false;
        Debug.Log("Collision handler reset");
    }
}
