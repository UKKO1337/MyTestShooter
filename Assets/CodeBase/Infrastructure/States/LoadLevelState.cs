using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.States
{
  public class LoadLevelState : IGamePayloadedState<string>
  {
    private IGameStateMachine _stateMachine;
    private SceneLoader _sceneLoader;
    private LoadingCurtain _curtain;



    [Inject]
    private void Construct(IGameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain)
    {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _curtain = curtain;
    }


    public void Enter(string sceneName)
    {
      _curtain.Show();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() =>
      _curtain.Hide();

    private void OnLoaded()
    {
      _stateMachine.Enter<GameLoopState>();
    }
    
    
  }
}