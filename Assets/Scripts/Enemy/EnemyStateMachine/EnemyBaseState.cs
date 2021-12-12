
/* ABSTRACT STATE : Here we define methods and variables concrete classes should inherit. Cannot create instances of this state. */
public abstract class EnemyBaseState
{
    private bool _isRootState = false;
    private EnemyStateMachine _context;
    private EnemyStateFactory _factory;
    private EnemyBaseState _currentSuperState;
    private EnemyBaseState _currentSubState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected EnemyStateMachine Context { get { return _context; } }
    protected EnemyStateFactory Factory { get { return _factory; } }


    public EnemyBaseState(EnemyStateMachine currentContext, EnemyStateFactory enemyStateFactory)
    {
        _context = currentContext;
        _factory = enemyStateFactory;
    }

    // create following methods for our concrete states

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void FixedUpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void InitializeSubState();

    public void FixedUpdateStates()
    {
        FixedUpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.FixedUpdateStates();
        }
    }

    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    public void ExitStates()
    {
        /*ExitState();
        if (_currentSubState != null)
        {
            _currentSubState.ExitStates();
        }*/
    }

    protected void SwitchState(EnemyBaseState newState)
    {
        // current state first exits state
        ExitState();

        // then enters a new state
        newState.EnterState();

        if (_isRootState)
        {
            // switch current state of context
            _context.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            // set the current super states sub state to the new state
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(EnemyBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(EnemyBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this); // substate setting itself as a superstate of its own new substate
    }
}
