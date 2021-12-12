using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This is a concrete state, an individual state that has behavior specific logic. It derives from the abstract state, Base state.
  It is also a sub-state of Groounded state and InAir state.
  It handles players "idle" behavior (when the player standing still)*/
public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base (currentContext, playerStateFactory) 
    {
        InitializeSubState();
    }

    // runs when game object first enters in this state 
    public override void EnterState() {
        // stop walking and running sounds
        Context.RunSound.Stop();
        Context.WalkSound.Stop();

    }

    // it is called from within PlayerStateMachine's update method while game object is in this state
    public override void UpdateState() 
    {
        Context.MoveSpeed = 0; // set move speed to 0 while not moving
        
        CheckSwitchStates();
    }

    // it is called from within PlayerStateMachine's fixedupdate method while game object is in this state
    public override void FixedUpdateState() { }

    // it is called when game object exits this state 
    public override void ExitState() { }

    // set substate
    public override void InitializeSubState() {
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
        if (Context.PlayerMoves && !Context.PlayerSprint)
        {
            SwitchState(Factory.Walk());
        }
        else if (Context.PlayerMoves && Context.PlayerSprint)
        {
            SwitchState(Factory.Sprint());
        }
    }
}
