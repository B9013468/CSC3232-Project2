using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This is a concrete state, an individual state that has behavior specific logic. It derives from the abstract state, Base state.
  It is also a sub-state of the sub states Sprint, Walk and Idle.
  It handles players "is attacked" behavior (when the player gets is attacked by an enemy)*/
public class PlayerInBattleState : PlayerBaseState
{
    public PlayerInBattleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) { }

    // runs when game object first enters in this state 
    public override void EnterState() 
    {
        Context.IsAttacked = true;
        Context.TimeNotAttacked = 0; // restart TimeNotAttacked to 0
    }

    // it is called from within PlayerStateMachine's update method while game object is in this state
    public override void UpdateState()
    {
        Context.MoveSpeedAdjuster = Context.AttackedSpeedAdjuster; // change speed adjuster to attacked speed

        // count the time the player has not been attacked 
        Context.TimeNotAttacked += Time.deltaTime;

        if (Context.TimeNotAttacked > 8)
        {
            Context.IsAttacked = false;
        }
        else if (Context.GetHit || Context.Attack)
        {
            Context.TimeNotAttacked = 0; // restart TimeNotAttacked to 0

            if (Context.GetHit)
            {
                Context.GetHit = false;
                Context.Health -= 10; // lose hp from hit
                Context.HpBar.GetComponent<HPBar>().AdjustHealth(Context.Health); // adjust hp on hp bar
                Vector3 directionOfHit = Context.PlayerRB.position - Context.Weapon.transform.position; // find the direction of hit
                Context.PlayerRB.AddForce(directionOfHit.normalized * 10, ForceMode.Impulse); // add knockback force to the player
            }
        }

        CheckSwitchStates();
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
        if (!Context.IsAttacked)
        {
            SwitchState(Factory.OutOfBattle());
        }
    }
}