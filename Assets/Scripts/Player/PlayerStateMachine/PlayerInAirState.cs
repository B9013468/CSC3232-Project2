using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This is a concrete state, an individual state that has behavior specific logic. It derives from the abstract state, Base state.
  It handles players "in air" behavior (when the player is in the air.*/
public class PlayerInAirState : PlayerBaseState
{
    // contstructor of superstate Jump
    public PlayerInAirState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) 
    {
        IsRootState = true; // make state to a super state
        InitializeSubState(); // call initilize sub state in the constructor, so the proper substate (Idle, Walk, Run) is created 
        }

    // runs when game object first enters in this state 
    public override void EnterState() 
    {
        Context.PlayerRB.useGravity = false; // disable gravity from rigidbody
        Context.PlayerRB.drag = Context.AirMoveDrag; // change rigidbody's drag while in the air so player won't move slow or fall slow

        if (Context.IsJumpPressed)
        {
            Jump(); // jump on entering state if jump has been pressed
        }
    }

    // it is called from within PlayerStateMachine's update method while game object is in this state
    public override void UpdateState() {
        
        // if player has jumped but has not double jumped yet, jump again 
        if(!Context.DoubleJump && Context.HasJumped && Context.IsJumpPressed)
        {
            Jump();
            Context.DoubleJump = true; // set double jump to true
        }

        CheckSwitchStates();
    }

    // it is called from within PlayerStateMachine's fixedupdate method while game object is in this state
    public override void FixedUpdateState() 
    {
        Context.PlayerRB.AddForce(Context.PlayerGravity, ForceMode.Acceleration); // change gravity while in the air so player move and fall smoothly and more realistically 
    }

    // it is called when game object exits this state 
    public override void ExitState() { }

    // set substate, depending on which of the following parameters are true (commented out playersprint substate because we want player to not be able to sprint while in the air)
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
        /*else if (Context.PlayerMoves && Context.PlayerSprint)
        {
            SetSubState(Factory.Sprint());
        }*/
    }

    public override void CheckSwitchStates() 
    {
        // if player has hit the ground, switch to grounded state
        if (Context.OnGround )
        {
            SwitchState(Factory.Grounded());
        }
    }

    void Jump()
    {
        Context.PlayerRB.velocity = new Vector3(Context.PlayerRB.velocity.x, 0f, Context.PlayerRB.velocity.z); // reset player's y velocity to 0 when jump 
        Context.PlayerRB.AddForce(Context.PlayerGO.transform.up * Context.JumpPower, ForceMode.Impulse); // add force upwards to jump 
    }
}
