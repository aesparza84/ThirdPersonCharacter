using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [SerializeField] private CharacterController charController;

    //private PlayerInputs inputs;
    private InputManager inputs;

    [Header("Camera")]
    [SerializeField] private CameraController camController;
    private Vector3 camForward;

    private bool aimMode;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Vector3 gravityVector;

    [Header("Gravity")]
    [SerializeField] private float gravity;
    [SerializeField] private float gravityAccel;
    [SerializeField] private float yVelocity;

    [Header("Jumping")]
    private bool jumpPressed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private int jumpCount;
    [SerializeField] private int maxJumpCount;


    private Vector3 velocity;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        inputs = GetComponent<InputManager>();
        camController = GetComponent<CameraController>();

        camController.OnAimMode += CamAimMode;
    }

    private void CamAimMode(object sender, bool e)
    {
        aimMode = e;
    }

    void Start()
    {
        camForward = Vector3.zero;
        direction = Vector3.zero;
        gravityVector = Vector3.zero;
        yVelocity = 0;

        jumpPressed = false;

        inputs.input.Player.MovementWASD.performed += MovementPerformed;
        inputs.input.Player.MovementWASD.canceled += MovementStopped;

        inputs.input.Player.JumpVault.performed += OnJump;
    }
    private void MovementStopped(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        direction = Vector3.zero;
    }
    private void MovementPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        direction.x = obj.ReadValue<Vector2>().x;
        direction.z = obj.ReadValue<Vector2>().y;
    }
    private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        jumpPressed = true;
    }
    private void ApplyGravity()
    {
        if (!charController.isGrounded)
        {
            yVelocity += gravity * gravityAccel * Time.deltaTime;
        }
        else
        {
            yVelocity = -1;
        }
    }

    void Update()
    {
        camForward.x = camController.GetAimPoint().x;
        camForward.z = camController.GetAimPoint().z;

        if (charController.isGrounded)
            jumpCount = 0;

        if (aimMode)
        {
            gameObject.transform.forward = camForward;
        }
        else
        {
            direction = (camController.ForwardRotation.right * direction.x) + (camController.ForwardRotation.forward * direction.z);
        }

        ApplyGravity();

        HandleJump();

        gravityVector.y = yVelocity;
        velocity = (direction.normalized + gravityVector) * speed * Time.deltaTime;

        charController.Move(velocity);
    }

    private void HandleJump()
    {
        if (jumpPressed)
        {
            if (charController.isGrounded)
            {
                yVelocity += jumpHeight;
                jumpCount++;
            }
            else if(jumpCount < maxJumpCount)
            {
                yVelocity += jumpHeight/2;
                jumpCount++;
            }
        }

        jumpPressed = false;
    }


    private void FixedUpdate()
    {
        
    }
}
