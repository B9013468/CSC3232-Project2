using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This is a concrete state, an individual state that has behavior specific logic. It derives from the abstract state, Base state.
  It is also a sub-state of the state InBattle.
  It handles enemy's "attacke" behavior (when the enemy attacks player)*/
public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory)
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
        Context.Agent.SetDestination(Context.Agent.transform.position); // agent stay in place 

        Vector3 lookPos = Context.Player.transform.position; // set look position as player position
        lookPos.y = Context.Agent.transform.position.y;
        Context.Agent.transform.LookAt(lookPos); // look at player

        // if enemy in attack range, call weapon animation
        if (Context.AttackMode)
        {
            Context.SwingSound.Play();
            Context.Weapon.GetComponent<EnemyAttack>().Hit();
            Context.AttackMode = false;
        }
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
        // if player is not in attack range, switch to chase state
        if (!Context.AttackPlayer)
        {
            SwitchState(Factory.Chase());
        }
    }
}