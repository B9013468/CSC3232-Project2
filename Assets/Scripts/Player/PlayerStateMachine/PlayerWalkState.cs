using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This is a concrete state, an individual state that has behavior specific logic. It derives from the abstract state, Base state.
  It is also a sub-state of Groounded state and InAir state.
  It handles players "walk" behavior (when the player walks)*/
public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) 
    {
        InitializeSubState();
    }

    // runs when game object first enters in this state 
    public override void EnterState() {
        // stop running sound and play walking sound
        Context.RunSound.Stop();
        Context.WalkSound.Play();
    }

    // it is called from within PlayerStateMachine's update method while game object is in this state
    public override void UpdateState()
    {
        CheckSwitchStates();

        Context.MoveSpeed = Mathf.Lerp(Context.MoveSpeed, Context.NormalSpeed, Context.AccelerateAdjuster * Time.deltaTime);
    }

    // it is called from within PlayerStateMachine's fixedupdate method while game object is in this state
    public override void FixedUpdateState()
    {
        if (Context.OnGround)
        {
            // add force in move direction, using normalized to make sure the magnitude is equal to 1 in all directions and using forcemode acceleration, adding continuous acceleration to our rigidbody, ignoring its mass
            // also multiply force with moveSpeed and moveSpeedAdjuster, to have the desired speed
            Context.PlayerRB.AddForce(Context.MoveDirection.normalized * Context.MoveSpeed * Context.MoveSpeedAdjuster, ForceMode.Acceleration);
        }
        else
        {
            // multiply addforce with airSpeedAdjuster so player won't move faster while on air
            Context.PlayerRB.AddForce(Context.MoveDirection.normalized * Context.MoveSpeed * Context.MoveSpeedAdjuster * Context.AirSpeedAdjuster, ForceMode.Acceleration);
        }
    }

    // it is called when game object exits this state 
    public override void ExitState() { }

    // set substate
    public override void InitializeSubState()
    {
        if (Context.IsAttacked || Context.Attack)
        {
            SetSubState(Factory.InBattle());
        }
        else if (!Context.IsAttacked)
        {
            SetSubState(Factory.OutOfBattle());
        }
    }

    // switch between sub states depending on parameters 
    public override void CheckSwitchStates()
    {
        if (!Context.PlayerMoves && !Context.PlayerSprint)
        {
            SwitchState(Factory.Idle());
        }
        else if (Context.PlayerMoves && Context.PlayerSprint)
        {
            SwitchState(Factory.Sprint());
        }
    }
}
