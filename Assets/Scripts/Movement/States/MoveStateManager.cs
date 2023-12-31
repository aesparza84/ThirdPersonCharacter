using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class MoveStateManager : MonoBehaviour
{
    /// <summary>
    /// This is the context. This will pass data to the seperate
    /// instances of 'MovingState' so that it can handle it differently.
    /// </summary>
    /// 

    [Header("Player Components")]
    public Rigidbody PlayerBody;
    [SerializeField] private Transform playerTransform;

    [Header("Camera")]
    [SerializeField] private ThirdPersonCamera followCam;
    private Transform direction;

    [Header("Animator")]
    public Animator MyAnimator; //Ref to the attatched Animator component

    [Header("Player control inputs")]
    public InputManager PlayerInputs; //Reference to the InputManager component

    [Header("Player Movement Stats")]
    public float BaseSpeed;
    public float Currentspeed;
    public float SprintSpeed;
    public float CrouchSpeed;

    [SerializeField] private Vector3 moveVector;

    public float HorizontalInput;
    public float VerticalInput;
    [SerializeField] private float inputZeroCheck;


    [SerializeField] private MovingState currentState;

    public WalkingState walkingState;
    public RunningState runnningState;
    public CrouchWalkState crouchWalkState;
    public IdleState idleState;
    public CrouchState crouchState;

    public bool crouched;

    public event EventHandler<MoveStateManager> StartedSprint;
    public event EventHandler<MoveStateManager> StoppedSprint;
    public event EventHandler<MoveStateManager> StartedWalking;
    public event EventHandler<MoveStateManager> StoppedWalking;
    public event EventHandler<MoveStateManager> StartedCrouch;
    public event EventHandler<MoveStateManager> StoppedCrouch;

    private void Awake()
    {
        if (PlayerInputs == null && TryGetComponent<InputManager>(out InputManager i))
        {
            PlayerInputs = i;
        }
        if (PlayerBody == null && TryGetComponent<Rigidbody>(out Rigidbody t))
        {
            PlayerBody = t;
        }

        if (MyAnimator == null && TryGetComponent<Animator>(out Animator a))
        {
            MyAnimator = a;
        }

        if (followCam == null && TryGetComponent<ThirdPersonCamera>(out ThirdPersonCamera c))
        {
            followCam = c;
        }

        if (playerTransform == null)
        {
            playerTransform = gameObject.transform;
        }

        walkingState = new WalkingState(this);
        runnningState = new RunningState(this);
        crouchWalkState = new CrouchWalkState(this);
        idleState = new IdleState(this);
        crouchState = new CrouchState(this); 
    }
    void Start()
    {
        moveVector = Vector3.zero;
        Currentspeed = 0;
        SprintSpeed = BaseSpeed * 2.3f; //A random number I chose
        CrouchSpeed = BaseSpeed * 0.4f; //A random number I chose
        crouched = false;

        PlayerInputs.input.Player.MovementWASD.performed += OnMovementPerformed; 
        PlayerInputs.input.Player.MovementWASD.canceled += OnMovementCancelled; 

        PlayerInputs.input.Player.Sprint.performed += OnSprintPerformed; 
        PlayerInputs.input.Player.Sprint.canceled += OnSprintCancelled;

        PlayerInputs.input.Player.Crouch.performed += OnCrouchPerformed;

        currentState = idleState;
        currentState.EnterState(this);
    }

    private void OnCrouchPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!crouched)
        {
            StartedCrouch.Invoke(this, this);            
        }
        else
        {
            StoppedCrouch.Invoke(this, this);
        }
    }

    #region Inputs
    private void OnSprintCancelled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StoppedSprint.Invoke(this, this);
    }

    private void OnSprintPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StartedSprint.Invoke(this, this);
    }

    private void OnMovementCancelled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StoppedWalking.Invoke(this, this);  
        SetInputs(0,0);
    }

    private void OnMovementPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //If the previous input was 0, then we will start walking from IDLE
        if (inputZeroCheck == 0)
        {
            StartedWalking.Invoke(this, this);
        }

        SetInputs(obj.ReadValue<Vector2>().x, obj.ReadValue<Vector2>().y);       
    }

    private void SetInputs(float x, float y)
    {
        HorizontalInput = x;
        VerticalInput = y;  
    }
    #endregion

    void Update()
    {
        Debug.Log(currentState);

        inputZeroCheck = (HorizontalInput * 2) + (VerticalInput * 1.7f);
        SetPlayersForward();
        currentState.DoUpdateAction(this);
    }

    private void SetPlayersForward()
    {
        direction = followCam.forwaredRotation;
        if (moveVector != Vector3.zero)
        {
            //playerTransform.forward = Vector3.Slerp(playerTransform.forward, direction.forward.normalized, Time.deltaTime); Aiming style
            playerTransform.forward = Vector3.Slerp(playerTransform.forward, moveVector.normalized, Time.deltaTime * 10);
        }
    }

    private void FixedUpdate()
    {
        movement();
    }

    private void movement()
    {
        if (direction)
        {
            moveVector = (direction.right * HorizontalInput) + (direction.forward * VerticalInput);
        }
        PlayerBody.velocity = moveVector * Currentspeed;
    }

    public void switctStates(MovingState passedState)
    {
        currentState.ExitState(this);
        currentState = passedState;
        currentState.EnterState(this);
    }
}
