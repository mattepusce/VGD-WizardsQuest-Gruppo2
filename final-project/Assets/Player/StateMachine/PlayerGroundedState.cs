using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{

    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) 
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsJumpPressed && !Ctx.RequireNewJumpPress)               // utilizza il getter per sapere se il tasto per saltare è premuto
        {
            SwitchState(Factory.Jump());                                // se è premuto, allora cambia stato dal corrente (grounded) a Jump
        }
    }

    public override void EnterState()
    {
        Ctx.CurrentMovementY = Ctx.GroundedGravity;
        Ctx.AppliedMovementY = Ctx.GroundedGravity;
    }

    public override void ExitState()
    {
        
    }

    public override void InitializeSubState()
    {
        if(!Ctx.IsMovementPressed && !Ctx.IsRunPressed)             // se non si premoto i tasti per muoversi o correre, setta la subState in Idle
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)        // se si premono i tasti per muoversi, setta la subState in Walk
        {
            SetSubState(Factory.Walk());
        }
        else                                                        // se si premono i tasti per muoversi e correre, setta la subState in Run
        {
            SetSubState(Factory.Run());
        }
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

}
