using System;

namespace CodeBase.Infrastructure
{
  public interface IGamePayloadedState<TPayload> : IGameBaseState
  {
    void Enter(TPayload payload);
  }
}