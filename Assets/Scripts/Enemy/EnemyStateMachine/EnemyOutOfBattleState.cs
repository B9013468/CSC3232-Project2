using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*This is a concrete state, an individual state that has behavior specific logic. It derives from the abstract state, Base state.
  It handles enemy's "out of battle" behavior (when the enemy is not been yet attacked by the player or has seen the player)*/
public class EnemyOutOfBattleState : EnemyBaseState
{
    public EnemyOutOfBattleState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory) 
    {
        IsRootState = true; // make state to a super state
        InitializeSubState(); // call initilize sub state in the constructor
    }

    // runs when game object first enters in this state 
    public override void EnterState()
    {
        Context.Agent.speed = Context.WalkSpeed; // change agent's speed to walk speed
        Context.InBattle = false;
    }

    // it is called from within EnemyStateMachine's update method while game object is in this state
    public override void UpdateState()
    {
        CheckSwitchStates();

        if(Context.Enemy.velocity != Vector3.zero)
            Context.Enemy.velocity = Vector3.zero; // zeroing velocity for the situtation where enemy was hit while entering out of battle state

        if (!Context.DestinationPointStated) // if a destination point has not been stated yet, create a random destination
        {
            Vector3 randomDestinationPoint = Random.insideUnitSphere * Context.ViewRange; // get a random destination in a certain range
            randomDestinationPoint += Context.EnemyGO.transform.position; // add enemy's position in this point
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDestinationPoint, out hit, Context.ViewRange, 1); // get a close position in the navmeshs
            Context.FinalDirectionPoint = hit.position;
            Context.DestinationPointStated = true;
        }
        else if (Context.DestinationPointStated)
        {
            Context.Agent.SetDestination(Context.FinalDirectionPoint); // set agent to start walking to destination point
        }

        if (Context.DistanceLeft.magnitude <= 1f) // if agent is getting close enough to destination point set destinationPointStated to false so another point can be stated
        {
            Context.DestinationPointStated = false;
        }

        if (Context.CanSeePlayer)
        {
            Debug.Log("Can see player!!!");
        }
        else
        {
            Debug.Log("Cannot see player!!!");
        }
    }

    // it is called from within EnemyStateMachine's fixedupdate method while game object is in this state
    public override void FixedUpdateState() 
    {
        Collider[] lookForLayer = Physics.OverlapSphere(Context.Enemy.transform.position, Context.ViewRange, Context.PlayerLayer); // get an array of objects found that have PlayerLayer for layer inside the viewrange of the enemy

        // if the array is not empty that means something on that layer is found 
        if (lookForLayer.Length != 0)
        {
            Transform enemyTarget = lookForLayer[0].transform; // get the array's first instance's transform (in this case we have looked only for our player so only the player should be in this array)
            
            Vector3 directionToTarget = (enemyTarget.position - Context.Enemy.transform.position).normalized; // get the direction from the enemy to this target

            // if this direction to target is inside our enemy's view angle
            if (Vector3.Angle(Context.Enemy.transform.forward, directionToTarget) < Context.ViewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(Context.Enemy.transform.position, enemyTarget.position); // get the enemy's distance to the target

                if(!Physics.Raycast(Context.Enemy.transform.position, directionToTarget, distanceToTarget, Context.ObstructionLayers)) // if raycast is not hitting an object with a layer othe than player's layer
                {
                    Context.CanSeePlayer = true;
                }
                else
                {
                    Context.CanSeePlayer = false;
                }
            }
            else
            {
                Context.CanSeePlayer = false;
            }
        }
        else if (Context.CanSeePlayer)
        {
            Context.CanSeePlayer = false;
        }
    }

    // it is called when game object exits this state 
    public override void ExitState() { }

    // set substate
    public override void InitializeSubState() { }

    // switch between sub states depending on parameters
    public override void CheckSwitchStates()
    {
        // if enemy is hit or can see player switch to in battle state
        if (Context.CanSeePlayer || Context.IsHit)
        {
            SwitchState(Factory.InBattle());
        }
        // if the enemy's hp dropped below 0, switch to dead state
        else if (Context.Health < 0)
        {
            SwitchState(Factory.Dead());
        }
    }
}