using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFactory         // crea lo stato a partire dal contesto
{
    PlayerStateMachine context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        this.context = currentContext;
    }

    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(context, this);
    }

    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(context, this);
    }

    public PlayerBaseState Run()
    {
        return new PlayerRunState(context, this);
    }

    public PlayerBaseState Jump()
    {
        return new PlayerJumpState(context, this);
    }

    public PlayerBaseState Grounded()
    {
        return new PlayerGroundedState(context, this);
    }

}
