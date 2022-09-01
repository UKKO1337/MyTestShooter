using System.Xml.Schema;
using UnityEngine;

namespace CodeBase.Services.Input
{
  public interface IInputService
  {
    Vector2 MoveAxis { get; }
    
    Vector2 LookAxis { get; }

    bool IsJumpButtonPressed();
    bool IsSprintButtonPressed();

    void Enable();
    void Disable();
  }
}