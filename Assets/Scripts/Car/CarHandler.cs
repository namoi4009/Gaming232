using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CarHandler : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Transform gameModel;

    [SerializeField]
    MeshRenderer carMeshRender;

    // Max values
    float maxSteerVelocity = 2;
    float maxFowardVelocity = 30;

    // Multiplier
    float accelerateMultiplier = 3f;
    float brakeMultiplier = 15f;
    float steeringMultiplier = 5f;

    // Input
    Vector2 input = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frames
    void Update()
    {
        // Rotate car when turning
        gameModel.transform.rotation = Quaternion.Euler(0, rb.velocity.x * 5, 0);
    }

    private void FixedUpdate()
    {
        // Execute Acceleration
        if (input.y > 0)
            Accelerate();
        else
            rb.drag = 0.2f;

        // Execute Brake
        if (input.y < 0)
            Brake();

        // Execute left and right moving
        Steer();

        // Avoid moving backward
        if (rb.velocity.z <= 0)
            rb.velocity = Vector3.zero;
    }

    void Accelerate()
    {
        rb.drag = 0;

        // If the velocity is too fast, stop accelerating
        if (rb.velocity.z >= maxFowardVelocity)
            return;

        // Accelerating
        rb.AddForce(rb.transform.forward * accelerateMultiplier * input.y);
    }

    void Brake()
    {
        // Don't brake if the car stand still
        if (rb.velocity.z <= 0)
            return;

        rb.AddForce(rb.transform.forward * brakeMultiplier * input.y);
    }

    void Steer()
    {
        if (Mathf.Abs(input.x) > 0)
        {
            // If we go slow, we steer slow
            float speedBaseSteerLimit = rb.velocity.z / 0.5f;
            speedBaseSteerLimit = Mathf.Clamp01(speedBaseSteerLimit);
            
            // steering
            rb.AddForce(rb.transform.right * steeringMultiplier * input.x * speedBaseSteerLimit);

            // Normalize the X Velocity
            float normalizedX = rb.velocity.x / maxSteerVelocity;

            // Ensure magnitude <= 1
            normalizedX = Mathf.Clamp(normalizedX, -1.0f, 1.0f);

            // Make sure out steering speed is under the limit
            rb.velocity = new Vector3(normalizedX * maxSteerVelocity, 0, rb.velocity.z);
        }
        else
        {
            // Auto center car
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, 0 ,rb.velocity.z), Time.fixedDeltaTime * 3);

        }
    }

    public void setInput(Vector2 inputVector)
    {
        inputVector.Normalize();

        input = inputVector;
    }
}