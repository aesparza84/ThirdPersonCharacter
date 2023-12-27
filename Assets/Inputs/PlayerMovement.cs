using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody myBody;
    private PlayerInputs input; //My custom action map

    private Vector3 moveVector;
    [SerializeField] private float BaseSpeed;
    [SerializeField] private float SprintSpeed;
    [SerializeField] private float CurrentSpeed;

    #region Initialization
    private void Awake()
    {
        input = new PlayerInputs();
    }

    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody>();
        moveVector = Vector3.zero;

        SprintSpeed = BaseSpeed * 2.3f;
        CurrentSpeed = BaseSpeed;
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.MovementWASD.performed += OnMovementPerformed;
        input.Player.MovementWASD.canceled += OnMovementCancelled;

        input.Player.Sprint.performed += OnSprintPerformed;
        input.Player.Sprint.canceled += OnSprintCancelled; ;
    }

    private void OnSprintCancelled(InputAction.CallbackContext obj)
    {
        CurrentSpeed = BaseSpeed;
    }

    private void OnSprintPerformed(InputAction.CallbackContext obj)
    {
        CurrentSpeed = SprintSpeed;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.MovementWASD.canceled -= OnMovementCancelled;
        input.Player.MovementWASD.performed -= OnMovementPerformed;
    }
    #endregion

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        myBody.velocity = moveVector * CurrentSpeed;
    }

    private void OnMovementPerformed(InputAction.CallbackContext obj)
    {
        moveVector = obj.ReadValue<Vector3>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext obj) 
    {
        moveVector = Vector3.zero;
    }
}
