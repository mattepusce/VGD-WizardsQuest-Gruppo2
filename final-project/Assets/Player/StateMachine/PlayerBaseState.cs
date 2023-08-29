
public abstract class PlayerBaseState 
{
    private bool isRootState = false;
    private PlayerStateMachine ctx;           // ctx fornisce informazioni sul contesto corrente del player
    private PlayerStateFactory factory;

    private PlayerBaseState currentSuperState;
    private PlayerBaseState currentSubState;

    protected bool IsRootState { set { isRootState = value; } }
    protected PlayerStateMachine Ctx { get { return ctx; } }
    protected PlayerStateFactory Factory { get { return factory; } }

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        this.ctx = currentContext;
        this.factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void UpdateStates() 
    {
        UpdateState();
        if(currentSubState != null)
        {
            currentSubState.UpdateStates();
        }
    }
    protected void SwitchState(PlayerBaseState newState)
    {
        ExitState();                    // esce dal current state
        newState.EnterState();          // entra nello state passato come parametro

        if (isRootState) { 
            ctx.CurrentState = newState;    //utilizza il setter CurrentState per settare il valore
        }
        else if(currentSuperState != null)
        {
            currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        currentSuperState = newSuperState;
    }
    
        
    protected void SetSubState(PlayerBaseState newSubState) 
    {
        currentSubState = newSubState;
        newSubState.SetSuperState(this);            // setta se stesso come superState
    }


}
