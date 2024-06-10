using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    // Instance
    private static InputManager instance;
    public static InputManager Instance 
    { 
        get
        {
            if (instance == null)
                instance = new InputManager();
            return instance;
        }
    }

    // Move Vector
    [SerializeField] private Vector2 moveInput;
    public Vector2 MoveInput { get => moveInput; }

    // Reset signal
    [SerializeField] private bool isPressingR;
    public bool IsPressingR { get => isPressingR; }

    private void Awake()
    {
        instance = this;
    }

    private void Reset()
    {
        this.LoadComponent();
    }

    private void LoadComponent()
    {
        moveInput = Vector2.zero;
        isPressingR = false;
    }

    void Update()
    {
        this.GetInput();
        this.GetResetSignal();
    }

    private void GetInput()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
    }

    private void GetResetSignal()
    {
        if (Input.GetKeyDown(KeyCode.R)) isPressingR = true;
        else isPressingR = false;
    }
}
