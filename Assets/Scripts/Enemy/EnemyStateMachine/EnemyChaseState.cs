using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This is a concrete state, an individual state that has behavior specific logic. It derives from the abstract state, Base state.
  It is also a sub-state of the state InBattle.
  It handles enemy's "chase" behavior (when the enemy chases the player)*/
public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory)
    {
        InitializeSubState(); // call initilize sub state in the constructor
    }

    // runs when game object first enters in this state 
    public override void EnterState()
    {

    }

    // it is called from within EnemyStateMachine's update method while game object is in this state
    public override void UpdateState()
    {
        Debug.Log("Chase State");
        Context.Agent.SetDestination(Context.Player.position); // set destination to player's position

        CheckSwitchStates();
    }

    // it is called from within EnemyStateMachine's fixedupdate method while game object is in this state
    public override void FixedUpdateState() { }

    // it is called when game object exits this state 
    public override void ExitState() { }

    // set substate
    public override void InitializeSubState() { }

    // switch between sub states depending on parameters
    public override void CheckSwitchStates()
    {
        // if enemy is in attack range attack player
        if (Context.AttackPlayer)
        {
            SwitchState(Factory.Attack());
        }
    }
}