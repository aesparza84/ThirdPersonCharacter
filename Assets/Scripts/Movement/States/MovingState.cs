using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingState
{
    //In each instance, we define the functionality differently
    protected PlayerMovement player;

    protected Animator animator;
    protected Rigidbody playerBody;
    protected float CurrentSpeed;
    public MovingState NextState { get; set; }
    public MovingState PrevState { get; set; }

    public abstract void DoUpdateAction(MoveStateManager context);
    public abstract void EnterState(MoveStateManager context);
    public abstract void ExitState(MoveStateManager context);
}
