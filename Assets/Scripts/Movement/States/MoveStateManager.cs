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

    [Header("Player Movement Stats/Script")]
    [SerializeField] private PlayerMovement player;

    [SerializeField] private MovingState currentState;

    private WalkingState walkingState;
    private RunningState runnningState;
    private IdleState idleState;
    private CrouchState crouchState;


    private void Awake()
    {
        if (player == null && TryGetComponent<PlayerMovement>(out PlayerMovement t))
        {
            player = t;
        }

        walkingState = new WalkingState(player, player.myAnimator);
        runnningState = new RunningState(player, player.myAnimator);
        idleState = new IdleState(player, player.myAnimator);
        crouchState = new CrouchState(player, player.myAnimator); 
    }
    void Start()
    {
        currentState = idleState;
        currentState.EnterState();
    }

    void Update()
    {
        currentState.DoUpdateAction();
    }

    private void switctStates(MovingState passedState)
    {
        currentState.ExitState();
        currentState = passedState;
        currentState.EnterState();
    }
}
