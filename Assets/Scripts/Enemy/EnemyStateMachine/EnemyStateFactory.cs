
/* Here all concrete states are created from within the context*/
public class EnemyStateFactory
{
    EnemyStateMachine _context;

    // Create factory's constructor function where the reference of the state machine is going to be hold
    public EnemyStateFactory(EnemyStateMachine currentContext)
    {
        _context = currentContext;
    }

    // Create public methods for each of the concrete states

    public EnemyOutOfBattleState OutOfBattle()
    {
        return new EnemyOutOfBattleState(_context, this);
    }
    public EnemyInBattleState InBattle()
    {
        return new EnemyInBattleState(_context, this);
    }
    public EnemyChaseState Chase()
    {
        return new EnemyChaseState(_context, this);
    }
    public EnemyAttackState Attack()
    {
        return new EnemyAttackState(_context, this);
    }
    public EnemyDeadState Dead()
    {
        return new EnemyDeadState(_context, this);
    }
}
