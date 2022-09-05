using System.Xml.Schema;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Services.Input
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