using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVault : PlayerState
{
    private Vector3 vaultPos;
    private bool doneVaulting;
    public PlayerVault(PlayerMoveManager passedContext, PlayerMoveFactory passedFactory) : base(passedContext, passedFactory)
    {
        doneVaulting = false;   
    }

    public override void CheckSwitchConditions()
    {
        if (doneVaulting)
        {
            SwitchToState(_factory.Idle());
        }
    }

    public override void ChooseSubState()
    {

    }

    public override void EnterState()
    {
        RaiseAimEvent(false);

        _context.JumpedVaultPressed = false;
        _context.Currentspeed = 0;
        _context.ToggleColliders(false,false);

        vaultPos = _context.CoverRayCast.GetVaultPoint();
        ToggleAnimationBool(true);

        _context.StartCoroutine(Vault(_context.PlayerBody, _context.PlayerBody.position, vaultPos, 1.5f));
    }

    public override void ExitState()
    {
        RaiseAimEvent(true);

        ToggleAnimationBool(false);
        _context.ToggleColliders(true, false);
    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        CheckSwitchConditions();

        //Old implementation;

        //Vector3 currentPos = _context.PlayerBody.position;
        //Vector3 newpos = Vector3.Lerp(currentPos, vaultPos, Time.deltaTime * 0.4f);
        //_context.PlayerBody.MovePosition(newpos);
        
    }

    private IEnumerator Vault(Rigidbody playerBody, Vector3 playerPos, Vector3 finalPos, float lerpSpeed)
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

        doneVaulting = true;
    }

    protected override void ToggleAnimationBool(bool toggle)
    {
        _context.MyAnimator.SetBool("Vaulting", toggle);
    }


}
