using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingState : MovingState
{
    bool doneClimbing;

    public ClimbingState(MoveStateManager context)
    {
        this.managerContext = context;
    }
    public override void DoFixedUpate(MoveStateManager context)
    {

    }

    public override void DoUpdateAction(MoveStateManager context)
    {
        if (doneClimbing)
        {
            context.switctStates(context.idleState);
        }
    }

    public override void EnterState(MoveStateManager context)
    {
        doneClimbing = false;
        ClimbWall();
    }

    private void ClimbWall()
    {
        Vector3 newPos = managerContext.coverRayCast.GetClimbPoint();
        managerContext.StartCoroutine(climbWall(managerContext.PlayerBody,
            managerContext.PlayerBody.position, 
            newPos, 
            1.0f));
    }
    private IEnumerator climbWall(Rigidbody playerBody, Vector3 playerPos, Vector3 finalPos, float lerpSpeed)
    {
        float t = 0;
        Vector3 currentPos = playerPos;

        while (t < 1)
        {
            playerPos = Vector3.Lerp(currentPos, finalPos, t);
            t += Time.deltaTime * lerpSpeed;
            playerBody.MovePosition(playerPos);

            yield return null;
        }

        doneClimbing = true;
    }

    public override void ExitState(MoveStateManager context)
    {

    }
}
