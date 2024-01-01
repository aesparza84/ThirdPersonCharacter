using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverRaycast : MonoBehaviour
{
    [Header("Player Input")]
    [SerializeField] private InputManager playerInput;

    private void Awake()
    {
        //Find and set ref to InputManager if null
        if (playerInput == null && TryGetComponent<InputManager>(out InputManager t))
        {
            playerInput = t;
        }
    }

    void Start()
    {
        playerInput.input.Player.Cover.performed += OnCoverPressed;
    }

    private void OnCoverPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //TODO: add cover logic ex: raycasting and allat'
    }

    void Update()
    {
        
    }
}
