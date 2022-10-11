using CodeBase.Services.PersistentProgress;

namespace CodeBase.Infrastructure.States
{
  public class MainMenuState : IGameState
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;

    public MainMenuState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain curtain)
    {
      _curtain = curtain;
      _gameStateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      
    }

    public void Enter()
    {
      _curtain.Hide();
      _sceneLoader.Load("Main_menu");
      _gameStateMachine.Enter<GameLoopState>();
    }

    public void Exit()
    {
      
    }
    
  }
}