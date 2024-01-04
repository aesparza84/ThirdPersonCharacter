using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverState : MovingState
{
    private RigidbodyConstraints prevConstraints; //Default Player-rigidbody constraints
    private RigidbodyConstraints lockOnX, lockOnZ;

    private Vector3 cancelledAxis;
    public CoverState(MoveStateManager context)
    {
        lockOnX = RigidbodyConstraints.FreezePositionX;
        lockOnZ = RigidbodyConstraints.FreezePositionZ;
        context.StoppedCover += OnLeaveCover;

        UsesFixedUpdt = true;
    }

    private void OnLeaveCover(object sender, MoveStateManager e)
    {
        if (active)
        {
            Debug.Log("Left Cover");
            e.inCover = false;
            e.switctStates(e.idleState);
        }
    }

    public override void DoUpdateAction(MoveStateManager context)
    {
        ///Shoot out RayCasts at Players-Collider's width (extents?)
        ///to detect if we passed a wall's edge
        ///


        RaycastHit hit;

        Vector3 rayPosLeft = new Vector3(context.transform.position.x + -context.playerCollider.bounds.extents.x,
                                         context.transform.position.y + context.playerCollider.bounds.extents.y);

        Vector3 rayPosRight = new Vector3(context.transform.position.x + context.playerCollider.bounds.extents.x,
                                          context.transform.position.y + context.playerCollider.bounds.extents.y);
        
        Debug.DrawRay(rayPosLeft, context.gameObject.transform.forward, Color.magenta);
        Debug.DrawRay(rayPosRight, context.gameObject.transform.forward, Color.magenta);
    }

    public override void DoFixedUpate(MoveStateManager context)
    {
        context.moveVector = context.direction.right * context.HorizontalInput + 
                             context.direction.up * context.VerticalInput;
        context.moveVector = Vector3.Project(context.moveVector, cancelledAxis);
        context.PlayerBody.velocity = context.moveVector.normalized * context.Currentspeed;
    }

    public override void EnterState(MoveStateManager context)
    {
        //We enter the 'covered' state
        active = true;

        MoveToCover(context);


        context.inCover = true;
        context.Currentspeed = context.CoverSpeed;

        prevConstraints = context.PlayerBody.constraints;

        //Check the cover ray cast normal hit
        //Restrict RigidBody to perpdenicular axis
        if (context.coverRayCast.NormalOnX)
        {
            Debug.Log("Stopping X movement");
            cancelledAxis = Vector3.right;
        }
        else if (context.coverRayCast.NormalOnZ)
        {
            Debug.Log("Stopping Z movement");
            cancelledAxis = Vector3.up;
        }

        
    }
    private void MoveToCover(MoveStateManager context)
    {
        context.PlayerBody.MovePosition(context.coverRayCast.CoverPoint);
        Debug.Log("Move to " + context.coverRayCast.CoverPoint);
    }

    public override void ExitState(MoveStateManager context)
    {
        context.coverRayCast.NormalOnX = false;
        context.coverRayCast.NormalOnZ = false;
        context.PlayerBody.constraints = prevConstraints;

    }
}
