using UnityEngine;

namespace CodeBase.Services.Input
{
  public interface IInputService : IService

  {
    Vector2 MoveAxis { get; }

  Vector2 LookAxis { get; }

  bool IsJumpButtonPressed();
  bool IsCrouchButtonPressed();
  bool IsSprintButtonPressed();
  bool IsZoomButtonPressed();
  bool IsShootButtonPressed();
  bool IsSaveButtonPressed();

  void Enable();
  void Disable();
  }
}