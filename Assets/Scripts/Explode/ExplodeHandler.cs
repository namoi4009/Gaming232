using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeHandler : MonoBehaviour
{

    [SerializeField]
    GameObject originalObject;

    [SerializeField]
    GameObject model;

    Rigidbody[] rigidbodies;

    private void Awake()
    {
        rigidbodies = model.GetComponentsInChildren<Rigidbody>(true);
    }

    public void Explode(Vector3 externalForce)
    {
        originalObject.SetActive(false);
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.transform.parent = null;
            rb.GetComponent<BoxCollider>().enabled = true;

            rb.gameObject.SetActive(true);
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
                        
            // Parts explode upward direction
            rb.AddForce(Vector3.up * 200 + externalForce, ForceMode.Force);
            // Parts explode forward direction
            rb.AddForce(Vector3.forward * 90 + externalForce, ForceMode.Force);
            // Parts rotate when explode
            rb.AddTorque(Random.insideUnitSphere * 0.5f, ForceMode.Impulse);

            // Change the tag so other object can explode after being hit by a car part
            rb.gameObject.tag = "CarPart";
        }
    }
}
