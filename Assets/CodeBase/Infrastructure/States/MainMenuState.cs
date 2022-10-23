using CodeBase.Services.PersistentProgress;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.States
{
  public class MainMenuState : IGameState
  {
    private IGameStateMachine _gameStateMachine;
    private SceneLoader _sceneLoader;
    private LoadingCurtain _curtain;


    [Inject]
    private void Construct(IGameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain curtain)
    {
      _curtain = curtain;
      _gameStateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
    }

    public void Enter()
    {
      ResumeGame();
      _curtain.Hide();
      _sceneLoader.Load("Main_menu");
      _gameStateMachine.Enter<GameLoopState>();
    }

    public void Exit()
    {
      
    }
    
    private void ResumeGame() => 
      Time.timeScale = 1;
  }
}