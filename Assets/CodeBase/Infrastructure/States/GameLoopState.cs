using Zenject;

namespace CodeBase.Infrastructure.States
{
  public class GameLoopState : IGameState
  {
    private IGameStateMachine _stateMachine;

    [Inject]
    private void Construct(IGameStateMachine stateMachine)
    {
      _stateMachine = stateMachine;
    }
    

    public void Exit()
    {
      
    }

    public void Enter()
    {
      
    }
  }
}