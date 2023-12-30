using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingState
{
    //In each instance, we define the functionality differently
    protected PlayerMovement player;

    protected Animator animator;
    protected Rigidbody playerBody { get; set; }
    protected float CurrentSpeed { get; set; }
    public MovingState NextState { get; set; }
    public MovingState PrevState { get; set; }

    public abstract void DoUpdateAction();
    public abstract void SwitchToState();

    public abstract void EnterState();
    public abstract void ExitState();
}
