using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody myBody;
    //private PlayerInputs input; //My custom action map
    private InputManager inputHandler;

    private Vector3 moveVector;
    //private Vector2 moveVector;
    private Transform direction;
    private Transform prevDirection;


    [SerializeField] private float BaseSpeed;
    [SerializeField] private float SprintSpeed;
    [SerializeField] private float CurrentSpeed;

    private float HorizontalInput;
    private float VerticalInput;

    [SerializeField] private ThirdPersonCamera followCam;

    #region Initialization
    private void Awake()
    {
        //input = new PlayerInputs();
        inputHandler = GetComponent<InputManager>();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void Start()
    {
        if (myBody == null)
        {
            myBody = GetComponent<Rigidbody>();
        }

        if (followCam == null)
        {
            followCam = GetComponentInChildren<ThirdPersonCamera>();
        }


        moveVector = Vector2.zero;

        SprintSpeed = BaseSpeed * 2.3f;
        CurrentSpeed = BaseSpeed;

        inputHandler.OnMovementPerf += MovementPerformed;
        inputHandler.OnMovementCanc += MovementCancelled;
        inputHandler.OnSprintPerf += SprintPerformed;
        inputHandler.OnSprintCanc += SprintCancelled;
    }

    private void SprintCancelled(object sender, System.EventArgs e)
    {
        CurrentSpeed = BaseSpeed;
    }

    private void SprintPerformed(object sender, System.EventArgs e)
    {
        CurrentSpeed = SprintSpeed;
    }

    private void MovementCancelled(object sender, System.EventArgs e)
    {
        moveVector = Vector2.zero;
    }

    private void MovementPerformed(object sender, Vector2 e)
    {
        //moveVector = e;
        HorizontalInput = e.x;
        VerticalInput = e.y;
        //moveVector = new Vector3(HorizontalInput, 0, VerticalInput);
        moveVector = (direction.right * HorizontalInput) + (direction.forward * VerticalInput);
        
    }
    #endregion

    void Update()
    {
        direction = followCam.forwaredRotation;
        if (prevDirection != direction)
        {

        }        
        prevDirection = direction;

        
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {        
        myBody.velocity =  moveVector * CurrentSpeed;
    }

    #region Original Input
    //private void OnSprintCancelled(InputAction.CallbackContext obj)
    //{
    //    CurrentSpeed = BaseSpeed;
    //}

    //private void OnSprintPerformed(InputAction.CallbackContext obj)
    //{
    //    CurrentSpeed = SprintSpeed;
    //}
    //private void OnMovementPerformed(InputAction.CallbackContext obj)
    //{
    //    //moveVector = obj.ReadValue<Vector3>();

    //}

    //private void OnMovementCancelled(InputAction.CallbackContext obj) 
    //{
    //    moveVector = Vector2.zero;
    //}
    #endregion
}
