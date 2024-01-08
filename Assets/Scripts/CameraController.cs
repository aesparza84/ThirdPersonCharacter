using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("The Players Cameras")]
    [SerializeField] private CinemachineFreeLook ThirdPersonCam;
    [SerializeField] private CinemachineVirtualCamera AimRightCam;
    //[SerializeField] private CinemachineVirtualCamera AimLeftCam;

    [Header("Reference to Player Input")]
    [SerializeField] private InputManager playerInputs;

    private void Awake()
    {
        if (ThirdPersonCam == null)
        {
            Debug.LogWarning("No ThirdPersonCam detected");
        }
        if (AimRightCam == null)
        {
            Debug.LogWarning("No AimRightCam detected");
        }

        if (playerInputs == null)
        {
            Debug.LogWarning("No Player Input referenced");
        }
    }
    void Start()
    {
        ThirdPersonCam.Priority = 10;
        AimRightCam.Priority = 0;

        playerInputs.input.Player.Aim.performed += OnAimPerformed;
        playerInputs.input.Player.Aim.canceled += OnAimStopped; 
    }

    private void OnAimStopped(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        ThirdPersonCam.Priority = 10;
        AimRightCam.Priority = 0;
    }

    private void OnAimPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        ThirdPersonCam.Priority = 0;
        AimRightCam.Priority = 10;
    }

    void Update()
    {
        
    }
}
