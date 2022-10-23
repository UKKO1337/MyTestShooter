using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Input;
using Zenject;

namespace CodeBase.Infrastructure
{
  public class Game
  {
    public IGameStateMachine StateMachine;

    [Inject]
    private void Construct(IGameStateMachine stateMachine)
    {
      StateMachine = stateMachine;
    }
  }
}