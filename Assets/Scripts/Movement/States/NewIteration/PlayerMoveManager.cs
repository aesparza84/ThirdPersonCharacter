using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveManager : MonoBehaviour
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
    [SerializeField] private Rigidbody playerBody;
    public Rigidbody PlayerBody { get { return playerBody; } set { playerBody = value; } }

    [SerializeField] private Transform playerTransform;
    public Transform PlayerTransform { get { return playerTransform; } set { playerTransform = value; } }

    [SerializeField] private Collider standingCollider;
    public Collider StandingCollider { get { return standingCollider; } set { standingCollider = value; } }

    [SerializeField] private Collider crouchingCollider;
    public Collider CrouchingCollider { get { return crouchingCollider; } set { crouchingCollider = value; } }

    [SerializeField] private Transform physicalBodyTransform;
    public Transform PhyscialBodyTransfom { get { return physicalBodyTransform; } set { physicalBodyTransform = value; } }

    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundMask;

    private float colliderWidth;
    private float colliderHeight;
    public float ColliderWidth { get { return colliderWidth; } }
    public float ColliderHeight { get { return colliderHeight; } }

    [Header("Camera")]
    [SerializeField] private CameraController camController;
    private Transform direction;
    private Vector3 camForward;

    public CameraController CamController { get { return camController; } }
    public Vector3 CamForward { get { return camForward; } }

    [Header("Cover Raycaster")]
    [SerializeField] private CoverRaycast coverRayCast;
    public CoverRaycast CoverRayCast { get { return coverRayCast; } }

    [Header("Animator")]
    [SerializeField] private Animator myAnimator; //Ref to the attatched Animator component
    public Animator MyAnimator { get { return myAnimator; } }

    [Header("Player control inputs")]
    [SerializeField] private InputManager playerInputs; //Reference to the InputManager component

    [Header("Player Movement Stats")]
    [SerializeField] private float baseSpeed;
    public float BaseSpeed { get { return baseSpeed; } set { baseSpeed = value; } }

    [SerializeField] private float currentspeed;
    public float Currentspeed { get { return currentspeed; } set { currentspeed = value; } }

    [SerializeField] private float sprintSpeed;
    public float SprintSpeed { get { return sprintSpeed; } set { sprintSpeed = value; } }

    [SerializeField] private float crouchSpeed;
    public float CrouchSpeed { get { return crouchSpeed; } set { crouchSpeed = value; } }

    [SerializeField] private float coverSpeed;
    public float CoverSpeed { get { return coverSpeed; } set { coverSpeed = value; } }

    [SerializeField] private float groundDrag;

    [SerializeField] private Vector3 moveVector;
    public Vector3 MoveVector { get { return moveVector; } set { moveVector = value; } }

    [SerializeField] private Vector3 gravityVector;
    public Vector3 GravityVector { get { return gravityVector; } set { gravityVector = value; } }

    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;
    private float inputZeroCheck;
    public float HorizontalIput { get { return horizontalInput; } set { horizontalInput = value; } }
    public float VerticalIput { get { return verticalInput; } }

    private float gravity;
    public float Gravity { get { return gravity; } set { gravity = value; } }

    [SerializeField] private float gravityMultiplier;
    public float GravityMultiplier { get { return gravityMultiplier; } set { gravityMultiplier = value; } }
    
    [SerializeField] private float maxGravityAmount;
    public float MaxGravityAmount { get { return maxGravityAmount; } }

    private float velocityOnY;
    public float VelocityOnY { get { return velocityOnY; }set { velocityOnY = value; } }

    private PlayerMoveFactory states;

    private PlayerState currentState;
    public PlayerState CurrentState { get { return currentState; } set { currentState = value; } }  


    [SerializeField]private bool crouchPressed;
    private bool coverPressed;
    [SerializeField]private bool crouchedCover;
    private bool crouched;
    private bool aimMode;
    private bool isMoving;
    private bool runPressed;
    private bool isGrounded;
    private bool jumpedVaultPressed;
    public bool IsMoving { get { return isMoving; } }
    public bool CrouchPressed { get { return crouchPressed; } set { crouchPressed = value; } }
    public bool CrouchedCover { get { return crouchedCover; } set { crouchedCover = value; } }
    public bool Crouched { get { return crouched; } set { crouched = value; } }
    public bool RunPressed { get { return runPressed; } }
    public bool CoverPressed { get { return coverPressed; }  set { coverPressed = value; } }
    public bool IsGrounded { get { return isGrounded; } }
    public bool AimMode { get { return aimMode; } }
    public bool JumpedVaultPressed { get { return jumpedVaultPressed; } set { jumpedVaultPressed = value; } }

    private float jumpVaultResetTimer;

    //public event EventHandler<MoveStateManager> StartedSprint;
    //public event EventHandler<MoveStateManager> StoppedSprint;
    //public event EventHandler<MoveStateManager> StartedWalking;
    //public event EventHandler<MoveStateManager> StoppedWalking;
    //public event EventHandler<MoveStateManager> StartedCrouch;
    //public event EventHandler<MoveStateManager> StoppedCrouch;
    //public event EventHandler<MoveStateManager> StartedCover;
    //public event EventHandler<MoveStateManager> StoppedCover;
    //public event EventHandler<MoveStateManager> OnClimb;



    private void Awake()
    {
        if (playerInputs == null && TryGetComponent<InputManager>(out InputManager i))
        {
            playerInputs = i;
        }
        if (playerBody == null && TryGetComponent<Rigidbody>(out Rigidbody t))
        {
            playerBody = t;
        }
        if (standingCollider == null || standingCollider == null)
        {
            Debug.LogWarning("Collider reference missing");
        }

        if (myAnimator == null && TryGetComponent<Animator>(out Animator a))
        {
            myAnimator = a;
        }

        if (camController == null && TryGetComponent<CameraController>(out CameraController s))
        {
            camController = s;
        }

        if (groundCheckTransform == null)
        {
            Debug.LogWarning("No groundcheck set");
        }

        if (coverRayCast == null && TryGetComponent<CoverRaycast>(out CoverRaycast v))
        {
            coverRayCast = v;
        }

        if (playerTransform == null)
        {
            playerTransform = gameObject.transform;
        }

        //We set the default state
        states = new PlayerMoveFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Start()
    {
        moveVector = Vector3.zero;
        currentspeed = 0;
        sprintSpeed = baseSpeed * 2.3f; //A random number I chose
        crouchSpeed = baseSpeed * 0.85f; //A random number I chose

        crouchPressed = false;
        coverPressed = false;
        runPressed = false;
        isMoving = false;
        jumpedVaultPressed = false;
        crouchedCover = false;

        if (StandingCollider != null)
        {
            colliderWidth = StandingCollider.bounds.extents.x;
            colliderHeight = StandingCollider.bounds.extents.y;
        }

        camForward = Vector3.zero;

        gravity = -9.81f;
        gravityVector = Vector3.zero;

        playerInputs.input.Player.MovementWASD.performed += OnMovementPerformed;
        playerInputs.input.Player.MovementWASD.canceled += OnMovementCancelled;

        playerInputs.input.Player.Sprint.performed += OnSprintPerformed;
        playerInputs.input.Player.Sprint.canceled += OnSprintCancelled;

        playerInputs.input.Player.Crouch.performed += OnCrouchPressed;

        playerInputs.input.Player.Cover.performed += OnCoverPressed;

        playerInputs.input.Player.Aim.performed += OnAim;
        playerInputs.input.Player.Aim.canceled += OnAimStopped;

        playerInputs.input.Player.JumpVault.performed += OnJumpVault;
    }

    private void OnJumpVault(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!jumpedVaultPressed)
        {
            jumpedVaultPressed = true;
            jumpVaultResetTimer = 0.1f;
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
        if (!coverPressed)
        {
            coverPressed = true;
        }
    }
    private void OnCrouchPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!crouchPressed)
        {
            crouchPressed = true;
        }
    }

    #region Inputs
    private void OnSprintCancelled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        runPressed = false;
    }

    private void OnSprintPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        runPressed = true;
    }

    private void OnMovementCancelled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isMoving = false;
        SetInputs(0, 0);
    }

    private void OnMovementPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //If the previous input was 0, then we will start walking from IDLE
        isMoving = true;
        SetInputs(obj.ReadValue<Vector2>().x, obj.ReadValue<Vector2>().y);
    }

    private void SetInputs(float x, float y)
    {
        horizontalInput = x;
        verticalInput = y;
    }
    #endregion

    void Update()
    {
        //Debug.Log("Is moving "+isMoving);

        direction = camController.ForwardRotation;

        camForward.x = camController.GetAimPoint().x;
        camForward.z = camController.GetAimPoint().z;

        isGrounded = checkGrounded();

        if (jumpedVaultPressed)
        {
            jumpVaultResetTimer -= Time.deltaTime;
            if (jumpVaultResetTimer < 0)
            {
                jumpVaultResetTimer = 0.0f;
                jumpedVaultPressed = false;
            }
        }

        SetPlayersForward();
        currentState.Update();
        //playerTransform.forward = CamController.GetAimPoint();

        Debug.DrawRay(gameObject.transform.position, moveVector, Color.red);
    }

    private void SetPlayersForward()
    {
        //direction = followCam.forwaredRotation;
        if (aimMode)
        {
            //This faces player to camera forward, AIM CAMERA
            //playerTransform.forward = direction.forward;
            //playerTransform.forward = camForward;
            playerTransform.forward = camForward;
        }
        else
        {
            if (moveVector != Vector3.zero)
            {
                playerTransform.forward = Vector3.Slerp(playerTransform.forward,
                    new Vector3(moveVector.x, 0, moveVector.z).normalized,
                    Time.deltaTime * 10);
            }
        }
    }

    private void FixedUpdate()
    {
        //If the current state changes movment completely,
        //we use the current state's-FixedUpdate rigidbody movement
        //instead of the default

        currentState.FixedUpdate();
        //movement();
    }

    private void movement()
    {
        moveVector = (camController.ForwardRotation.right * horizontalInput) + (camController.ForwardRotation.forward * verticalInput);

        playerBody.velocity = moveVector.normalized * currentspeed; //normalized to ensure equal length vector for directions
    }

    private void applyGravity()
    {
        gravityVector.y += gravity * gravityMultiplier;
        if (gravityVector.y < -maxGravityAmount)
        {
            gravityVector.y = gravity;
        }

        playerBody.velocity += gravityVector;
    }

    private bool checkGrounded()
    {
        return Physics.CheckSphere(groundCheckTransform.position, 0.3f, groundMask);
    }

    public void ToggleColliders(bool enableStand, bool enableCrouch)
    {
        standingCollider.enabled = enableStand;
        crouchingCollider.enabled = enableCrouch;
    }

    public bool CanClimb() { return coverRayCast.CheckIfClimbable(); }
    public bool CanVault() { return coverRayCast.CheckIfVaultable(); }
}
