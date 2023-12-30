using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] private Rigidbody myBody;
    [SerializeField] private Transform playerTransform;

    [Header("Vector2 Input-Values")]
    [SerializeField] private float HorizontalInput;
    [SerializeField] private float VerticalInput;

    //private PlayerInputs input; //My custom action map
    private InputManager inputHandler;

    private Vector3 moveVector;
    //private Vector2 moveVector;
    private Transform direction;

    [Header("Player-Movement Fields")]
    [SerializeField] private float BaseSpeed;
    [SerializeField] private float SprintSpeed;
    [SerializeField] private float CurrentSpeed;
    [Header("Camera")]
    [SerializeField] private ThirdPersonCamera followCam;

    [Header("Animator")]
    [SerializeField] private Animator myAnimator;
    
    [Header("Move State")]
    [SerializeField] private MovementStates movingState;
    private MovementStates prevState;

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
        if (myBody == null && TryGetComponent<Rigidbody>(out Rigidbody n))
        {
            myBody = n;
        }

        if (followCam == null && TryGetComponent<ThirdPersonCamera>(out ThirdPersonCamera t))
        {
            followCam = t;
        }

        if (playerTransform == null)
        {
            playerTransform = gameObject.transform;
        }

        if (myAnimator == null && TryGetComponent<Animator>(out Animator a))
        {
            myAnimator = a;
        }

        moveVector = Vector3.zero;
        movingState = MovementStates.Idle;

        SprintSpeed = BaseSpeed * 2.3f;
        CurrentSpeed = BaseSpeed;

        //Custom input handling
        //inputHandler.OnMovementPerf += MovementPerformed;
        //inputHandler.OnMovementCanc += MovementCancelled;
        //inputHandler.OnSprintPerf += SprintPerformed;
        //inputHandler.OnSprintCanc += SprintCancelled;

        //Input managers Own handling
        inputHandler.input.Player.MovementWASD.performed += OnMovementPerformed;
        inputHandler.input.Player.MovementWASD.canceled += OnMovementCancelled;

        inputHandler.input.Player.Sprint.performed += OnSprintPerformed;
        inputHandler.input.Player.Sprint.canceled += OnSprintCancelled;
    }

    #region InputManager's Own Events
    private void OnSprintCancelled(InputAction.CallbackContext obj)
    {
        movingState = MovementStates.Walking;
    }

    private void OnSprintPerformed(InputAction.CallbackContext obj)
    {
        movingState = MovementStates.Running;
    }

    private void OnMovementCancelled(InputAction.CallbackContext obj)
    {
        movingState = MovementStates.Idle;
        SetInputs(0,0);
    }

    private void OnMovementPerformed(InputAction.CallbackContext obj)
    {
        movingState = MovementStates.Walking;
        SetInputs(obj.ReadValue<Vector2>().x, obj.ReadValue<Vector2>().y);
    }
    #endregion

    #endregion

    #region My custon inputManager Events
    //private void SprintCancelled(object sender, System.EventArgs e)
    //{
    //    //Debug.Log("Stopped Sprinting");
    //    CurrentSpeed = BaseSpeed;
    //    movingState = MovementStates.Walking;
    //}

    //private void SprintPerformed(object sender, System.EventArgs e)
    //{
    //    //Debug.Log("Sprinting");
    //    movingState = MovementStates.Running;
    //    CurrentSpeed = SprintSpeed;
    //}

    //private void MovementCancelled(object sender, System.EventArgs e)
    //{
    //    SetInputs(0,0);
    //    movingState = MovementStates.Idle;
    //}

    //private void MovementPerformed(object sender, Vector2 e)
    //{
    //    SetInputs(e.x, e.y);
    //    movingState = MovementStates.Walking;
    //}
    #endregion

    private void SetInputs(float x, float y)
    {
        HorizontalInput = x;
        VerticalInput = y;
    }

    private void checkWalking()
    {
        if (HorizontalInput == 0 && VerticalInput == 0)
        {
            movingState = MovementStates.Idle;
        }
    }
    

    void Update()
    {
        direction = followCam.forwaredRotation;
        if (moveVector != Vector3.zero)
        {
            //playerTransform.forward = Vector3.Slerp(playerTransform.forward, direction.forward.normalized, Time.deltaTime); Aiming style
            playerTransform.forward = Vector3.Slerp(playerTransform.forward, moveVector.normalized, Time.deltaTime * 10);
        }
        checkWalking();

        switch (movingState)
        {
            case MovementStates.Idle:
                myAnimator.SetBool("IsWalking", false);
                break;
            case MovementStates.Walking:
                myAnimator.SetBool("IsRunning", false);
                myAnimator.SetBool("IsWalking", true);
                CurrentSpeed = BaseSpeed;

                break;
            case MovementStates.Running:
                myAnimator.SetBool("IsRunning", true);
                CurrentSpeed = SprintSpeed;

                break;
            case MovementStates.CrouchIdle:
                break;
            case MovementStates.CrouchWalking:
                break;
            case MovementStates.Jumping:
                break;
            case MovementStates.Falling:
                break;
            default:
                break;
        }

        prevState = movingState;

        #region Debugging
        //Debug.Log("Move Vector "+moveVector);
        //Debug.Log("Current Speed "+CurrentSpeed);
        Debug.Log("MovState " + movingState);
        #endregion
    }

    private void FixedUpdate()
    {
        Movement();
        
    }

    private void Movement()
    {
        if (direction)
        {
            moveVector = (direction.right * HorizontalInput) + (direction.forward * VerticalInput);
        }
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
