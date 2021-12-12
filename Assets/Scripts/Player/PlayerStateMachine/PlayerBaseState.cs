
/* ABSTRACT STATE : Here we define methods and variables concrete classes should inherit. Cannot create instances of this state. */
public abstract class PlayerBaseState
{
    private bool _isRootState = false;
    private PlayerStateMachine _context;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSuperState;
    private PlayerBaseState _currentSubState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected PlayerStateMachine Context { get { return _context; } }
    protected PlayerStateFactory Factory { get { return _factory; } }


    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _context = currentContext;
        _factory = playerStateFactory;
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
        if(_currentSubState != null)
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

    protected void SwitchState(PlayerBaseState newState) 
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
        else if(_currentSuperState != null)
        {
            // set the current super states sub state to the new state
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerBaseState newSuperState) 
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState) 
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this); // substate setting itself as a superstate of its own new substate
    }
}
