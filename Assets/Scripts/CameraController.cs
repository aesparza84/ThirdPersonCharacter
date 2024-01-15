using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("The Players Cameras")]
    [SerializeField] private CinemachineVirtualCameraBase thirdPersonCam;
    [SerializeField] private CinemachineVirtualCameraBase aimCamera;
    //[SerializeField] private CinemachineVirtualCameraBase AimLeftCam;
    private Camera mainCam;

    private CinemachineVirtualCameraBase CurrentCamera;

    [SerializeField] private Transform followTarget;

    public Transform ForwardRotation;
    public Vector3 camViewDirection; //Facing towards player

    [SerializeField] private float MouseSensitivity;
    private float defaultMouseSensitivity;

    private Vector2 mouseVector;
    private float xInput;
    private float yInput;

    private Vector2 screenCenter;
    private Vector3 aimPoint;

    bool aimMode;

    [SerializeField] private Transform debubTransform;

    [Header("Reference to Player Input")]
    [SerializeField] private InputManager playerInputs;


    private void Awake()
    {
        if (thirdPersonCam == null)
        {
            Debug.LogWarning("No ThirdPersonCam detected");
        }
        if (aimCamera == null)
        {
            Debug.LogWarning("No AimRightCam detected");
        }

        if (followTarget == null)
        {
            Debug.LogWarning("No Follow Target");
        }

        if (playerInputs == null)
        {
            Debug.LogWarning("No Player Input referenced");
        }
    }
    void Start()
    {
        mainCam = Camera.main;

        aimCamera.gameObject.SetActive(false);
        CurrentCamera = thirdPersonCam;

        playerInputs.input.Player.Aim.performed += OnAimPerformed;
        playerInputs.input.Player.Aim.canceled += OnAimStopped;

        playerInputs.input.Player.MouseAIm.performed += OnMouseMoved;

        defaultMouseSensitivity = MouseSensitivity;
        aimMode = false;
    }

    private void OnMouseMoved(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        mouseVector = obj.ReadValue<Vector2>();
        yInput += mouseVector.x * (MouseSensitivity/10); 
        xInput += mouseVector.y * (MouseSensitivity/10);        
    }

    private void OnAimStopped(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        thirdPersonCam.gameObject.SetActive(true);
        aimCamera.gameObject.SetActive(false);

        aimMode = false;

        CurrentCamera = thirdPersonCam;
    }

    private void OnAimPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        thirdPersonCam.gameObject.SetActive(false);
        aimCamera.gameObject.SetActive(true);

        aimMode = true;

        CurrentCamera = aimCamera;
    }

    void Update()
    {
        //aimPoint = Vector3.zero;

        //screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        //Ray ray = mainCam.ScreenPointToRay(screenCenter);

        //aimPoint = ray.direction * 50f;

        ////if (Physics.Raycast(ray, out RaycastHit hit, 50))
        ////{
        ////    aimPoint = hit.point;
        ////    if (debubTransform != null)
        ////    {
        ////        debubTransform.position = hit.point;
        ////    }
        ////}

        //camViewDirection = (gameObject.transform.position - new Vector3(CurrentCamera.transform.position.x,
        //                 gameObject.transform.position.y, CurrentCamera.transform.position.z)).normalized;

        


        //MouseSensitivity = defaultMouseSensitivity;
        //if (aimMode)
        //{
        //    camViewDirection = (new Vector3(aimPoint.x, 0, aimPoint.z) - new Vector3(gameObject.transform.position.x,
        //                 0, gameObject.transform.position.z)).normalized;

        //    debubTransform.position = aimPoint;

        //    MouseSensitivity /= 2;
        //}

        //rotateVirtualCam();

        //ForwardRotation.forward = camViewDirection;

        //Debug.DrawRay(CurrentCamera.transform.position, camViewDirection * 10, Color.magenta);
    }

    //Since we are using fixedUpdate movement, Camera must be the same
    private void FixedUpdate()
    {
        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        aimPoint = Vector3.zero;

        camViewDirection = (gameObject.transform.position - new Vector3(CurrentCamera.transform.position.x,
                         gameObject.transform.position.y, CurrentCamera.transform.position.z)).normalized;


        MouseSensitivity = defaultMouseSensitivity;
        if (aimMode)
        {
            Ray ray = mainCam.ScreenPointToRay(screenCenter);

            Debug.DrawRay(CurrentCamera.transform.position, ray.direction * 50f, Color.magenta);

            aimPoint = mainCam.ScreenToWorldPoint(screenCenter) + ray.direction * 50f;

            camViewDirection = (new Vector3(aimPoint.x, 0, aimPoint.z) - new Vector3(gameObject.transform.position.x,
                         0, gameObject.transform.position.z)).normalized;

            if (debubTransform != null)
            {
                debubTransform.position = aimPoint;
            }

            MouseSensitivity /= 2;
        }

        rotateVirtualCam();

        ForwardRotation.forward = camViewDirection;

        //Debug.DrawRay(CurrentCamera.transform.position, camViewDirection * 10, Color.magenta);
    }

    private void rotateVirtualCam()
    {
        xInput = Mathf.Clamp(xInput, -30, 70);
        Quaternion rotation = Quaternion.Euler(-xInput, yInput, 0f);
        followTarget.rotation = rotation;
    }

    public Vector3 GetAimPoint()
    {
        return aimPoint;
    }
}
