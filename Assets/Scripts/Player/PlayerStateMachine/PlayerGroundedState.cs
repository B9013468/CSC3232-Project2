using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This is a concrete state, an individual state that has behavior specific logic. It derives from the abstract state, Base state.
  It handles players "grounded" behavior (when the player is on the gorund.*/
public class PlayerGroundedState : PlayerBaseState
{
    // contstructor of superstate Grounded
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory)
    {
        IsRootState = true; // make state to a super state
        InitializeSubState(); // call initilize sub state in the constructor, so the proper substate (Idle, Walk, Run) is created regardless which super state (Grounded, Jump) is currently active
    }

    // it is called when game object first enters in this state 
    public override void EnterState() 
    {
        // reset HasJumped and DoubleJump to false and rigidbody's gravity to true
        Context.HasJumped = false;
        Context.DoubleJump = false;
        Context.PlayerRB.useGravity = true;

        Context.PlayerRB.drag = Context.NormalDrag; // set rigidbody's drag to normal
    }

    // it is called from within PlayerStateMachine's update method while game object is in this state
    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    // it is called from within PlayerStateMachine's fixedupdate method while game object is in this state
    public override void FixedUpdateState() { }

    // it is called when game object exits this state 
    public override void ExitState() { }

    // set substate, depending on which of the following parameters are true
    public override void InitializeSubState() 
    { 
        if (!Context.PlayerMoves && !Context.PlayerSprint)
        {
            SetSubState(Factory.Idle());
        }
        else if (Context.PlayerMoves && !Context.PlayerSprint)
        {
            SetSubState(Factory.Walk());
        }
        else if (Context.PlayerMoves && Context.PlayerSprint)
        {
            SetSubState(Factory.Sprint());
        }
    }

    public override void CheckSwitchStates() 
    {
        // if player is not grounded or jump is pressed, switch to in air state
        if (!Context.OnGround || Context.IsJumpPressed)
        {
            SwitchState(Factory.InAir());
        }
    }
}
