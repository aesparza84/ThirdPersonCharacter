using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundMask;

    [Header("Camera")]
    [SerializeField] private CameraController camController;
    public Transform direction;
    public Vector3 camForward;

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
    [SerializeField] private float groundDrag;
    public Vector3 moveVector;
    private Vector3 rbVelocity;
    [SerializeField] private Vector3 gravityVector;

    public float HorizontalInput;
    public float VerticalInput;
    public float inputZeroCheck;

    private float gravity;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float maxGravityAmount;
    private float velocityOnY;


    [SerializeField] private MovingState currentState;

    public WalkingState walkingState;
    public RunningState runnningState;
    public CrouchWalkState crouchWalkState;
    public IdleState idleState;
    public CrouchState crouchState;
    public CoverState coverState;
    public ClimbingState climbState;

    public bool crouched;
    public bool inCover;
    private bool aimMode;
    private bool isMoving;

    public event EventHandler<MoveStateManager> StartedSprint;
    public event EventHandler<MoveStateManager> StoppedSprint;
    public event EventHandler<MoveStateManager> StartedWalking;
    public event EventHandler<MoveStateManager> StoppedWalking;
    public event EventHandler<MoveStateManager> StartedCrouch;
    public event EventHandler<MoveStateManager> StoppedCrouch;
    public event EventHandler<MoveStateManager> StartedCover;
    public event EventHandler<MoveStateManager> StoppedCover;
    public event EventHandler<MoveStateManager> OnClimb;

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

        if (groundCheckTransform == null)
        {
            Debug.LogWarning("No groundcheck set");
        }

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
        climbState = new ClimbingState(this);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Start()
    {
        moveVector = Vector3.zero;
        Currentspeed = 0;
        SprintSpeed = BaseSpeed * 2.3f; //A random number I chose
        CrouchSpeed = BaseSpeed * 0.85f; //A random number I chose
        crouched = false;
        inCover = false;

        camForward = Vector3.zero;

        gravity = -9.81f;
        gravityVector = Vector3.zero;

        PlayerInputs.input.Player.MovementWASD.performed += OnMovementPerformed; 
        PlayerInputs.input.Player.MovementWASD.canceled += OnMovementCancelled; 

        PlayerInputs.input.Player.Sprint.performed += OnSprintPerformed; 
        PlayerInputs.input.Player.Sprint.canceled += OnSprintCancelled;

        PlayerInputs.input.Player.Crouch.performed += OnCrouchPerformed;
        PlayerInputs.input.Player.Cover.performed += OnCoverPressed;

        PlayerInputs.input.Player.Aim.performed += OnAim;
        PlayerInputs.input.Player.Aim.canceled += OnAimStopped;

        PlayerInputs.input.Player.JumpVault.performed += OnJumpVault;

        currentState = idleState;
        currentState.EnterState(this);
    }

    private void OnJumpVault(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //TODOL Make these into states
        if (isMoving && CanVault())
        {
            Vector3 newpos = coverRayCast.GetVaultPoint();
            PlayerBody.MovePosition(newpos);
            Debug.Log("Vaulted");
        }
        else if (CanClimb())
        {
            switctStates(climbState);
        }
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
        isMoving = false;
        SetInputs(0,0);


    }

    private void OnMovementPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //If the previous input was 0, then we will start walking from IDLE
        if (inputZeroCheck == 0)
        {
            StartedWalking.Invoke(this, this);
        }
        isMoving = true;
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
        //Debug.Log(currentState);

        direction = camController.ForwardRotation;
        camForward = camController.ForwardRotation.forward;

        inputZeroCheck = (HorizontalInput * 2) + (VerticalInput * 1.7f); //My way of checking if the previous input is 0
        SetPlayersForward();
        currentState.DoUpdateAction(this);

        Debug.DrawRay(gameObject.transform.position, moveVector, Color.red);
    }

    private void SetPlayersForward()
    {
        //direction = followCam.forwaredRotation;
        if (aimMode)
        {
            //This faces player to camera forward, AIM CAMERA
            //playerTransform.forward = direction.forward;
            playerTransform.forward = camForward;
        }
        else
        {
            if (moveVector != Vector3.zero)
            {
                playerTransform.forward = Vector3.Slerp(playerTransform.forward,
                    new Vector3(moveVector.x,0,moveVector.z).normalized, 
                    Time.deltaTime * 10);
            }
        }
    }

    private void FixedUpdate()
    {
        //If the current state changes movment completely,
        //we use the current state's-FixedUpdate rigidbody movement
        //instead of the default

        rbVelocity = PlayerBody.velocity;

        if (currentState.UsesFixedUpdt) 
        {
            currentState.DoFixedUpate(this);
        }
        else
        {
            movement();
        }

        if (!grounded())
        {
            applyGravity();
        }
        else
        {
            gravityVector.y = 0;
        }
    }

    private void movement()
    {
        //if (direction)
        //{
            
        //}
        //moveVector = (direction.right * HorizontalInput) + (direction.forward * VerticalInput);
        moveVector = (camController.ForwardRotation.right * HorizontalInput) + (camController.ForwardRotation.forward * VerticalInput);
        
        PlayerBody.velocity = moveVector.normalized * Currentspeed; //normalized to ensure equal length vector for directions
    }

    private void applyGravity()
    {
        gravityVector.y += gravity * gravityMultiplier;
        if (gravityVector.y < -maxGravityAmount)
        {
            gravityVector.y = gravity;
        }

        PlayerBody.velocity += gravityVector;
    }

    private bool grounded()
    {
        return Physics.CheckSphere(groundCheckTransform.position, 0.3f, groundMask);
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

    public bool CanClimb() { return coverRayCast.CanClimb; }
    public bool CanVault() { return coverRayCast.CanVault; }
}
