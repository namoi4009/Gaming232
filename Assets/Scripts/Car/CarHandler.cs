using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CarHandler : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] Transform gameModel;

    [SerializeField] ExplodeHandler explodeHandler;

    IngameSound audioManager;

    private bool gameEnded = false;

    private bool win = false;

    public bool isGameEnded() { return gameEnded; }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Ingame_Sound").GetComponent<IngameSound>();
        audioManager.PlaySFX_Loop(audioManager.Ingame_Background);
    }

    // Max values
    float maxSteerVelocity = 2;
    float maxFowardVelocity = 40;

    // Multiplier
    float accelerateMultiplier = 3f;
    float brakeMultiplier = 15f;
    float steeringMultiplier = 5f;

    // Input
    Vector2 input = Vector2.zero;

    // Explode State
    private bool isExploded = false;

    bool isPlayer = true;

    // Force to Explode?
    private bool isForceToExplode = false;

    // Start is called before the first frame update
    void Start()
    {
        isPlayer = CompareTag("Player");
    }

    // Update is called once per frames
    void Update()
    {
        if (isForceToExplode && !gameEnded)
        {
            forceToExplode();
            gameEnded = true;
            return;
        }

        if (isExploded) return;

        // Rotate car when turning
        gameModel.transform.rotation = Quaternion.Euler(0, rb.velocity.x * 5, 0);

        setInput(InputManager.Instance.MoveInput);
    }

    private void FixedUpdate()
    {
        // Is exploded
        if (isExploded)
        {
            // Set the velocity to zero
            rb.velocity = Vector3.zero;

            // Move towad after exploded
            rb.MovePosition(Vector3.Lerp(transform.position, new Vector3(0, 0, transform.position.z), Time.deltaTime * 0.5f));

            return;
        }

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

    public void SetMaxSpeed(float newMaxSpeed)
    {
        maxFowardVelocity = newMaxSpeed;
    }

    public void SetForceToExplode()
    {
        isForceToExplode = true;
    }

    IEnumerator SlowDownTimeCO() // Slowdown time to end game
    {
        if (Time.timeScale > 0.2f)
        {
            Time.timeScale = 0.4f;
            yield return null;  
        }

        // Player achive higher score than last highest score
        
        yield return new WaitForSeconds(1.0f);  

        if (Time.timeScale == 0.4f)
            Time.timeScale = 1f;

        yield return new WaitForSeconds(0.5f);

        if (ScoreManager.Instance.isHigherScore())
        {
            // Enable Higher Score Panel
            ScoreUI.Instance.ViewHigherScorePanel();
            // Play Congrats sound
            audioManager.PlaySFX_OneTime(audioManager.Congrats);
            gameEnded = true;
            win = true;
        }
        if (!win)
        {
            audioManager.PlaySFX_OneTime(audioManager.LosingGame);
            gameEnded = true;
        }

        yield return new WaitForSeconds(1f);
        ScoreManager.Instance.setHigherScore();
        ScoreUI.Instance.ViewEndingPanel();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isPlayer)
        {
            // AI cars will only explode when they hit player car or car part
            if (collision.transform.root.CompareTag("Untagged"))
                return;

            if (collision.transform.root.CompareTag("CarAI"))
                return;
        }

        Vector3 velocity = rb.velocity;
        explodeHandler.Explode(velocity * 15);

        isExploded = true;

        audioManager.StopSFX();
        audioManager.PlaySFX_OneTime(audioManager.CarCrash);
        
        if (!gameEnded)
            StartCoroutine( SlowDownTimeCO() );
    }

    private void forceToExplode()
    {
        Vector3 velocity = rb.velocity;
        explodeHandler.Explode(velocity * 15);

        isExploded = true;

        audioManager.StopSFX();
        audioManager.PlaySFX_OneTime(audioManager.CarCrash);

        StartCoroutine( SlowDownTimeCO() );
    }
}