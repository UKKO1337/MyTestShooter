using CodeBase.Infrastructure.States;
using CodeBase.Services;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
  public class LoadFromButton : MonoBehaviour
  {
    private IGameStateMachine _stateMachine;
    private IInputService _inputService;
    private bool _isLoadGame;

    private void Awake()
    {
      _stateMachine = AllServices.Container.Single<IGameStateMachine>();
      _inputService = AllServices.Container.Single<IInputService>();
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