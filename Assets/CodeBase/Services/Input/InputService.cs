using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace CodeBase.Services.Input
{
  public class InputService : IInputService
  {
    private readonly PlayerInput _playerInput;

    public InputService() => 
      _playerInput = new PlayerInput();
    

    public Vector2 MoveAxis => 
      _playerInput.Player.Move.ReadValue<Vector2>();

    public Vector2 LookAxis => 
      _playerInput.Player.Look.ReadValue<Vector2>();

    public bool IsJumpButtonPressed() =>
      _playerInput.Player.Jump.IsPressed();

    public bool IsSprintButtonPressed() =>
      _playerInput.Player.Run.IsPressed();
    
    public void Enable() =>
      _playerInput.Enable();
    
    public void Disable() =>
      _playerInput.Disable();
  }
}