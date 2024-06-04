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

    void Start()
    {
        
    }

    public void Explode(Vector3 externalForce)
    {
        originalObject.SetActive(false);
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.transform.parent = null;
            rb.GetComponent<MeshCollider>().enabled = true;

            rb.gameObject.SetActive(true);
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            rb.AddForce(Vector3.up * 200 +  externalForce, ForceMode.Force);
            rb.AddTorque(Random.insideUnitSphere * 0.5f, ForceMode.Impulse);

            // Change the tag so other object can explode after being hit by a car part
            rb.gameObject.tag = "CarPart";
        }
    }
}
