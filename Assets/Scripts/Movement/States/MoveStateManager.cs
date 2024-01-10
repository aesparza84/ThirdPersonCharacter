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
    /// 
    /// All the seperate 'states' will access and manipulate all fields here seperately,
    /// which is why a lot of these must be public.
    /// 
    /// </summary>
    /// 

    [Header("Player Components")]
    public Rigidbody PlayerBody;
    [SerializeField] private Transform playerTransform;
    public Collider StandingCollider;
    public Collider CrouchingCollider;
    public Transform physicalBodyTransform;

    [Header("Camera")]
    [SerializeField] private CameraController camController;
    //[SerializeField] private ThirdPersonCamera followCam;
    public Transform direction;

    [Header("Cover Raycaster")]
    public CoverRaycast coverRayCast;

    [Header("Animator")]
    public Animator MyAnimator; //Ref to the attatched Animator component

    [Header("Player control inputs")]
    public InputManager PlayerInputs; //Reference to the InputManager component

    [Header("Player Movement Stats")]
    public float BaseSpeed;
    public float Currentspeed;
    public float SprintSpeed;
    public float CrouchSpeed;
    public float CoverSpeed;

    //[SerializeField] private Vector3 moveVector;
    public Vector3 moveVector;

    public float HorizontalInput;
    public float VerticalInput;
    public float inputZeroCheck;


    [SerializeField] private MovingState currentState;

    public WalkingState walkingState;
    public RunningState runnningState;
    public CrouchWalkState crouchWalkState;
    public IdleState idleState;
    public CrouchState crouchState;
    public CoverState coverState;

    public bool crouched;
    public bool inCover;
    private bool aimMode;

    public event EventHandler<MoveStateManager> StartedSprint;
    public event EventHandler<MoveStateManager> StoppedSprint;
    public event EventHandler<MoveStateManager> StartedWalking;
    public event EventHandler<MoveStateManager> StoppedWalking;
    public event EventHandler<MoveStateManager> StartedCrouch;
    public event EventHandler<MoveStateManager> StoppedCrouch;
    public event EventHandler<MoveStateManager> StartedCover;
    public event EventHandler<MoveStateManager> StoppedCover;

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
        if (StandingCollider == null || StandingCollider==null)
        {
            Debug.LogWarning("Collider reference missing");
        }

        if (MyAnimator == null && TryGetComponent<Animator>(out Animator a))
        {
            MyAnimator = a;
        }

        if (camController == null && TryGetComponent<CameraController>(out CameraController s))
        {
            camController = s;
        }

        //if (followCam == null && TryGetComponent<ThirdPersonCamera>(out ThirdPersonCamera c))
        //{
        //    followCam = c;
        //}

        if(coverRayCast == null && TryGetComponent<CoverRaycast>(out CoverRaycast v))
        {
            coverRayCast = v; 
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
        coverState = new CoverState(this);
    }
    void Start()
    {
        moveVector = Vector3.zero;
        Currentspeed = 0;
        SprintSpeed = BaseSpeed * 2.3f; //A random number I chose
        CrouchSpeed = BaseSpeed * 0.85f; //A random number I chose
        crouched = false;
        inCover = false;

        PlayerInputs.input.Player.MovementWASD.performed += OnMovementPerformed; 
        PlayerInputs.input.Player.MovementWASD.canceled += OnMovementCancelled; 

        PlayerInputs.input.Player.Sprint.performed += OnSprintPerformed; 
        PlayerInputs.input.Player.Sprint.canceled += OnSprintCancelled;

        PlayerInputs.input.Player.Crouch.performed += OnCrouchPerformed;
        PlayerInputs.input.Player.Cover.performed += OnCoverPressed;

        PlayerInputs.input.Player.Aim.performed += OnAim;
        PlayerInputs.input.Player.Aim.canceled += OnAimStopped;

        currentState = idleState;
        currentState.EnterState(this);
    }

    private void OnAimStopped(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        aimMode = false;
    }

    private void OnAim(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        aimMode = true;
    }

    private void OnCoverPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!inCover)
        {
            StartedCover.Invoke(this, this);
        }
        else
        {
            StoppedCover.Invoke(this, this);
        }
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

        inputZeroCheck = (HorizontalInput * 2) + (VerticalInput * 1.7f); //My way of checking if the previous input is 0
        SetPlayersForward();
        currentState.DoUpdateAction(this);

        Debug.DrawRay(gameObject.transform.position, moveVector, Color.red);
    }

    private void SetPlayersForward()
    {
        //direction = followCam.forwaredRotation;
        direction = camController.ForwardRotation;
        if (aimMode)
        {
            //This faces player to camera forward, AIM CAMERA
            //playerTransform.forward = direction.forward;
        }
        else
        {
            if (moveVector != Vector3.zero)
            {
                playerTransform.forward = Vector3.Slerp(playerTransform.forward, moveVector.normalized, Time.deltaTime * 10);
            }
        }
    }

    private void FixedUpdate()
    {
        //If the current state changes movment completely,
        //we use the current state's-FixedUpdate rigidbody movement
        //instead of the default

        if (currentState.UsesFixedUpdt) 
        {
            currentState.DoFixedUpate(this);
        }
        else
        {
            movement();
        }
    }

    private void movement()
    {
        if (direction)
        {
            moveVector = (direction.right * HorizontalInput) + (direction.forward * VerticalInput);
        }
        PlayerBody.velocity = moveVector.normalized * Currentspeed; //normalized to ensure equal length vector for directions
    }

    public void switctStates(MovingState passedState)
    {
        currentState.ExitState(this);
        currentState = passedState;
        currentState.EnterState(this);
    }

    public void ToggleColliders(bool enableStand, bool enableCrouch) 
    {
        StandingCollider.enabled = enableStand;
        CrouchingCollider.enabled = enableCrouch;
    }
}
