using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MovingState
{
    public override MovingState NextState { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public override MovingState PrevState { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public override void DoAction()
    {

    }

    public override void SwitchToState()
    {
        throw new System.NotImplementedException();
    }
}
