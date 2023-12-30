using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingState
{
    //In each instance, we define the functionality differently
    public abstract MovingState NextState { get; set; }
    public abstract MovingState PrevState { get; set; }

    public abstract void DoAction();
    public abstract void SwitchToState();
}
