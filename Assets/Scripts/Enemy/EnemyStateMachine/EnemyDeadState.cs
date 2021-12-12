using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This is a concrete state, an individual state that has behavior specific logic. It derives from the abstract state, Base state.
  It handles enemy's "dead" behavior (when the enemy lose all hp and dies)*/
public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory)
    {
        IsRootState = true; // make state to a super state
        InitializeSubState(); // call initilize sub state in the constructor
    }

    // runs when game object first enters in this state 
    public override void EnterState()
    {
        Context.InBattle = false;
        Context.DeathSound.Play();
    }

    // it is called from within EnemyStateMachine's update method while game object is in this state
    public override void UpdateState()
    {
        if (Context.KnockBack)
        {
            Context.KnockBack = false;
        }
    }

    // it is called from within EnemyStateMachine's fixedupdate method while game object is in this state
    public override void FixedUpdateState() { }

    // it is called when game object exits this state 
    public override void ExitState() { }

    // set substate
    public override void InitializeSubState() { }

    // switch between sub states depending on parameters
    public override void CheckSwitchStates() { }
}