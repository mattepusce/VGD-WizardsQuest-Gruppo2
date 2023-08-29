using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerJumpState : PlayerBaseState
{

    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) 
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void EnterState()
    {
        HandleJump();
    }

    public override void ExitState()
    {
        Ctx.Animator.SetBool("isJumping", false);

        if (Ctx.IsJumpPressed)
        {
            Ctx.RequireNewJumpPress = true;
        }
        
    }

    public override void InitializeSubState()
    {

    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleGravity();
    }

    void HandleJump()
    {
        Ctx.Animator.SetBool("isJumping", true);
        Ctx.IsJumping = true;
        Ctx.CurrentMovementY = Ctx.InitialJumpVelocity;
        Ctx.AppliedMovementY = Ctx.InitialJumpVelocity;
    }

    void HandleGravity()
    {
        bool isFalling = Ctx.CurrentMovementY <= 0f || !Ctx.IsJumpPressed;      // è true se il player sta cadendo o se il tast relativo al salto smette di essere premuto
        float fallMultiplier = 2f;                                              // la velocità di cadutà verrà moltiplicate per 2

        if (isFalling)
        {
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.Gravity * fallMultiplier * Time.deltaTime);
            Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * 0.5f, -20f);          // non moltiplica per Time.deltaTime perché currentMovement e currentRunMovement
                                                                                                                // vengono moltiplicati per Time.deltaTime direttamente nell'Update
                                                                                                                // il Max() serve a definire la velocity massima di caduta, ovvero -20
        }
        else
        {
            float previousYVelocity = Ctx.CurrentMovementY;                                     // velocity del momento
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.Gravity * Time.deltaTime);       // la velocity viene aggiornata tenendo conto della gravità
            Ctx.AppliedMovementY = (previousYVelocity + Ctx.CurrentMovementY) * 0.5f;           // velocity verlet integration, per consistenza (framerate indipendent)
        }
    }
          
    
}
