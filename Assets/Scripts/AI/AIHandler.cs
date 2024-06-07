using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]
    CarHandler carHandler;

    // Collistion detection
    [SerializeField]
    LayerMask otherCarsLayerMask;

    [SerializeField]
    MeshCollider meshCollider;

    RaycastHit[] raycastHits = new RaycastHit[1];
    bool isCarAhead = false;

    // Lanes
    int drivingInLane = 0;

    // Timing
    WaitForSeconds wait = new WaitForSeconds(0.2f);

    private void Awake()
    {
        if (CompareTag("Player"))
        {
            Destroy(this);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float accelerationInput = 1.0f;
        float steerInput = 0.0f;

        if (isCarAhead)
            accelerationInput = -1;

        float desiredPositionX = Utils.CarLanes[drivingInLane];
        float difference = desiredPositionX - transform.position.x;

        if (Mathf.Abs(difference) > 0.05f)
            steerInput = 1.0f * difference;

        steerInput = Mathf.Clamp(steerInput, -1.0f, 1.0f);
        carHandler.setInput(new Vector2(steerInput, accelerationInput));

    }

    IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            isCarAhead = checkIfOtherCarsIsAhead();
            yield return wait;
        }
    }

    bool checkIfOtherCarsIsAhead()
    {
        meshCollider.enabled = false;
        int numberOfHits = Physics.BoxCastNonAlloc(transform.position, Vector3.one * 0.25f, transform.forward, raycastHits, Quaternion.identity, 2, otherCarsLayerMask);
        meshCollider.enabled = true;

        if (numberOfHits > 0)
            return true;

        return false;
    }

    // Events
    private void OnEnable()
    {
        // Set random speed
        carHandler.SetMaxSpeed(Random.Range(2, 4));

        // Set random lane
        drivingInLane = Random.Range(0, Utils.CarLanes.Length);
    }
}
