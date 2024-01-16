using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveFactory 
{
    private PlayerMoveManager _context;
    public PlayerMoveFactory(PlayerMoveManager context)
    {
        _context = context;
    }

    /// <summary>
    /// When switching to a new state, we will call these methods to create
    /// a new one.
    /// </summary>
    /// <returns></returns>
    /// 

    public PlayerGrounded Grounded()
    {
        return new PlayerGrounded(_context, this);
    }
    public PlayerFall Fall()
    {
        return new PlayerFall(_context, this);
    }
    public PlayerIdle Idle() 
    {
        return new PlayerIdle(_context, this);
    }
    public PlayerWalk Walk() 
    {
        return new PlayerWalk(_context, this);
    }
    public PlayerRun Run() 
    {
        return new PlayerRun(_context, this);
    }
    public PlayerCrouch Crouch()
    {
        return new PlayerCrouch(_context, this);
    }
    public PlayerCover Cover()
    {
        return new PlayerCover(_context, this);
    }
    public PlayerVault Vault()
    {
        return new PlayerVault(_context, this);
    }
}
