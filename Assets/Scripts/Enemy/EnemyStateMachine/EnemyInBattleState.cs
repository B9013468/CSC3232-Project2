using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This is a concrete state, an individual state that has behavior specific logic. It derives from the abstract state, Base state.
  It handles enemy's "in battle" behavior (when the enemy is in battle mode with the player)*/
public class EnemyInBattleState : EnemyBaseState
{
    public EnemyInBattleState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) 
    {
        IsRootState = true; // make state to a super state
        InitializeSubState(); // call initilize sub state in the constructor, so the proper substate (Chase or Attack) is created
    }

    // runs when game object first enters in this state 
    public override void EnterState()
    {
        Context.Agent.speed = Context.ChaseSpeed; // change agent's speed to chase speed
        Context.InBattle = true;
    }

    // it is called from within EnemyStateMachine's update method while game object is in this state
    public override void UpdateState()
    {
         CheckSwitchStates();

        // if enemy is hit
        if (Context.IsHit)
        {
            Context.Health -= 10; // lose 10 hp
            Context.PositionWhereHit = Context.Enemy.transform.position; // get the position that enemy had when it got hit
            if(Context.Health > 0)
            {
                Context.IsHit = false;
                Context.Agent.isStopped = true; // stop enemy from moving
                Context.DirectionOfHit = Context.PlayerWeapon.transform.position - Context.Enemy.position; // get the direction of weapon's hit by substracting enemy's position from weapon's position
                Context.Enemy.AddForce(-Context.DirectionOfHit.normalized * 10, ForceMode.Impulse); // add force to enemy using the reverse direction of hit (so the enmy is knocked back, away from weapon)
                Context.HpBar.GetComponent<HPBar>().AdjustHealth(Context.Health); // adjust health bar to the remaining health
                Context.KnockBack = true;
            }
        }

        // if player is knocked back too far from the player, balance the force that was added to it from the hit, by zeroing velocity
        if ((Context.Enemy.transform.position - Context.PositionWhereHit).magnitude > 3 && Context.KnockBack)
        {
            Context.KnockBack = false;
            Context.Agent.isStopped = false;
            Context.Enemy.velocity = Vector3.zero;
        }
    }

    // it is called from within EnemyStateMachine's fixedupdate method while game object is in this state
    public override void FixedUpdateState() { }

    // it is called when game object exits this state 
    public override void ExitState() { }

    // set substate
    public override void InitializeSubState() 
    {
        if (!Context.AttackPlayer)
        {
            SetSubState(Factory.Chase());
        }
        else if (Context.AttackPlayer)
        {
            SetSubState(Factory.Attack());
        }
    }

    // switch between sub states depending on parameters
    public override void CheckSwitchStates()
    {
        // if enemy is too far from its last target position, or too far from player, switch to out of battle state
        if(Context.DistanceLeft.magnitude > Context.ChaseDistance || (Context.Enemy.transform.position - Context.Player.transform.position).magnitude > Context.ChaseDistance)
        {
            SwitchState(Factory.OutOfBattle());
        }
        // if the enemy's hp dropped below 0, switch to dead state
        else if(Context.Health < 0)
        {
            SwitchState(Factory.Dead());
        }
    }
}