using CodeBase.Infrastructure.States;
using CodeBase.Services;
using CodeBase.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
  public class LoadFromButton : MonoBehaviour
  {
    private IGameStateMachine _stateMachine;
    private IInputService _inputService;
    private bool _isLoadGame;

    
    
    [Inject]
    private void Construct(IInputService inputService, IGameStateMachine stateMachine)
    {
      _stateMachine = stateMachine;
      _inputService = inputService;
    }
    

    private void Update() => 
      LoadGame();

    private void LoadGame()
    {
      if (_inputService.IsLoadButtonPressed())
      {
        _isLoadGame = true;
        _stateMachine.Enter<BootstrapState, bool>(_isLoadGame);
      }
    }
  }
}