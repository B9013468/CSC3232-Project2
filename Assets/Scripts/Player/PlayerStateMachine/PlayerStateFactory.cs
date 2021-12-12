
/* Here all concrete states are created from within the context*/
public class PlayerStateFactory
{
    PlayerStateMachine _context;

    // Create factory's constructor function where the reference of the state machine is going to be hold
    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }

    // Create public methods for each of the concrete states

    public PlayerOutOfBattleState OutOfBattle()
    {
        return new PlayerOutOfBattleState(_context, this);
    }
    public PlayerInBattleState InBattle()
    {
        return new PlayerInBattleState(_context, this);
    }
    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(_context, this);
    }
    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(_context, this);
    }
    public PlayerBaseState Sprint()
    {
        return new PlayerSprintState(_context, this);
    }
    public PlayerBaseState InAir()
    {
        return new PlayerInAirState(_context, this);
    }
    public PlayerBaseState Grounded()
    {
        return new PlayerGroundedState(_context, this);
    }
}
