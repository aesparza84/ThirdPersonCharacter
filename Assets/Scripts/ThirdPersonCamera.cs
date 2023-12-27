using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References to Player")]
    [SerializeField] private GameObject Player;
    private InputManager inputHandler;

    private void Start()
    {
        inputHandler = GetComponentInParent<InputManager>();
    }

    Transform forwaredRotation;

    Vector3 viewDirection;
    private void Update()
    {
        viewDirection = (Player.transform.position - new Vector3(gameObject.transform.position.x, 
                         Player.transform.position.y, gameObject.transform.position.z) ).normalized;
   
        forwaredRotation.forward = viewDirection;
    }
}
