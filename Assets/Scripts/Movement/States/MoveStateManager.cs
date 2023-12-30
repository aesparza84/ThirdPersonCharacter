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

    [SerializeField] private MovingState currentState;

    private WalkingState walkingState;
    private RunningState runnningState;
    private IdleState idleState;
    private CrouchState crouchState;


    private void Awake()
    {
        walkingState = new WalkingState();
        runnningState = new RunningState();
        idleState = new IdleState();
        crouchState = new CrouchState(); 
    }
    void Start()
    {
        currentState = idleState;
    }

    void Update()
    {
        currentState.DoAction();
    }
}
