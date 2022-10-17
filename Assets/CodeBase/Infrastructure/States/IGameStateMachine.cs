using CodeBase.Services;

namespace CodeBase.Infrastructure.States
{
  public interface IGameStateMachine : IService
  {
    void Enter<TState, TParameter>(TParameter payload) where TState : class, IGamePayloadedState<TParameter>;
    void Enter<TState>() where TState : class, IGameState;
  }
}