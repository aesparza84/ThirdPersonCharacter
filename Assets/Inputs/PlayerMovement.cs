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
    [SerializeField] private Transform playerTransform;

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

        if (playerTransform == null)
        {
            playerTransform = gameObject.transform;
        }

        moveVector = Vector3.zero;

        SprintSpeed = BaseSpeed * 2.3f;
        CurrentSpeed = BaseSpeed;

        inputHandler.OnMovementPerf += MovementPerformed;
        inputHandler.OnMovementCanc += MovementCancelled;
        inputHandler.OnSprintPerf += SprintPerformed;
        inputHandler.OnSprintCanc += SprintCancelled;
    }

    private void SprintCancelled(object sender, System.EventArgs e)
    {
        //Debug.Log("Stopped Sprinting");
        CurrentSpeed = BaseSpeed;
    }

    private void SprintPerformed(object sender, System.EventArgs e)
    {
        //Debug.Log("Sprinting");
        CurrentSpeed = SprintSpeed;
    }

    private void MovementCancelled(object sender, System.EventArgs e)
    {
        SetInputs(0,0);
    }

    private void MovementPerformed(object sender, Vector2 e)
    {
        SetInputs(e.x, e.y);
    }

    private void SetInputs(float x, float y)
    {
        HorizontalInput = x;
        VerticalInput = y;
    }
    #endregion

    void Update()
    {
        direction = followCam.forwaredRotation;
        if (moveVector != Vector3.zero)
        {
            //playerTransform.forward = Vector3.Slerp(playerTransform.forward, direction.forward.normalized, Time.deltaTime); Aiming style
            playerTransform.forward = Vector3.Slerp(playerTransform.forward, moveVector.normalized, Time.deltaTime * 10);
        }

        #region Debugging
        //Debug.Log("Move Vector "+moveVector);
        //Debug.Log("Current Speed "+CurrentSpeed);
        #endregion
    }

    private void FixedUpdate()
    {
        Movement();
        
    }

    private void Movement()
    {        
        moveVector = (direction.right * HorizontalInput) + (direction.forward * VerticalInput);        
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
