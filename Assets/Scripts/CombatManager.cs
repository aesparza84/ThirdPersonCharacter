using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private InputManager PlayerInputs;

    [SerializeField] private CameraController camController;

    private Vector3 aimDirection;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInputs = GetComponent<InputManager>();
        camController = GetComponent<CameraController>();
        PlayerInputs.input.Player.Shoot.performed += OnShoot;
    }

    private void OnShoot(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        GameObject temop = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        aimDirection = camController.GetAimPoint() - shootPoint.position;
        shootPoint.forward = aimDirection;
    }
}
