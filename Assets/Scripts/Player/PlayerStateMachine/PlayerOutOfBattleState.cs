using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This is a concrete state, an individual state that has behavior specific logic. It derives from the abstract state, Base state.
  It is also a sub-state of the sub states Sprint, Walk and Idle.
  It handles players "not attacked" behavior (when the is not yet attacked by an enemy)*/
public class PlayerOutOfBattleState : PlayerBaseState
{
    public PlayerOutOfBattleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    // runs when game object first enters in this state 
    public override void EnterState() 
    {
        Context.TimeNotAttacked = 0;
    }

    // it is called from within PlayerStateMachine's update method while game object is in this state
    public override void UpdateState()
    {
        CheckSwitchStates();

        Context.MoveSpeedAdjuster = Context.NormalSpeedAdjuster;

    }

    // it is called from within PlayerStateMachine's fixedupdate method while game object is in this state
    public override void FixedUpdateState() { }

    // it is called when game object exits this state 
    public override void ExitState() { }

    // set substate
    public override void InitializeSubState() { }

    // switch between sub states depending on parameters
    public override void CheckSwitchStates()
    {
        if (Context.IsAttacked || (Context.Attack && !Levels.GamePaused))
        {
            SwitchState(Factory.InBattle());
        }
    }
}