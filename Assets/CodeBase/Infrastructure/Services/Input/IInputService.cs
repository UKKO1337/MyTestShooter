using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
  public interface IInputService : IService

  {
  Vector2 MoveAxis { get; }

  Vector2 LookAxis { get; }

  bool IsJumpButtonPressed();
  bool IsSprintButtonPressed();

  void Enable();
  void Disable();
  }
}