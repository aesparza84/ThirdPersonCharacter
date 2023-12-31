using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Vector3 MoveVector;

    public float HorizontalInput;
    public float VerticalInput;



    [SerializeField] private MovingState currentState;

    public WalkingState walkingState;
    public RunningState runnningState;
    public IdleState idleState;
    //public CrouchState crouchState;

    public event EventHandler<MoveStateManager> StartedSprint;
    public event EventHandler<MoveStateManager> StoppedSprint;
    public event EventHandler<MoveStateManager> StoppedWalking;

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
        idleState = new IdleState(this);
        //crouchState = new CrouchState(); 
    }
    void Start()
    {
        MoveVector = Vector3.zero;
        Currentspeed = 0;
        SprintSpeed = BaseSpeed * 2.3f; //A random number I chose

        PlayerInputs.input.Player.MovementWASD.performed += OnMovementPerformed; 
        PlayerInputs.input.Player.MovementWASD.canceled += OnMovementCancelled; 

        PlayerInputs.input.Player.Sprint.performed += OnSprintPerformed; 
        PlayerInputs.input.Player.Sprint.canceled += OnSprintCancelled; 

        currentState = idleState;
        currentState.EnterState(this);
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
        SetPlayersForward();
        currentState.DoUpdateAction(this);
    }

    private void SetPlayersForward()
    {
        direction = followCam.forwaredRotation;
        if (MoveVector != Vector3.zero)
        {
            //playerTransform.forward = Vector3.Slerp(playerTransform.forward, direction.forward.normalized, Time.deltaTime); Aiming style
            playerTransform.forward = Vector3.Slerp(playerTransform.forward, MoveVector.normalized, Time.deltaTime * 10);
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
            MoveVector = (direction.right * HorizontalInput) + (direction.forward * VerticalInput);
        }
        PlayerBody.velocity = MoveVector * Currentspeed;
    }

    public void switctStates(MovingState passedState)
    {
        currentState.ExitState(this);
        currentState = passedState;
        currentState.EnterState(this);
    }
}
