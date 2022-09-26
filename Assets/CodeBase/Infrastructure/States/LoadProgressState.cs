using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEditor;

namespace CodeBase.Infrastructure.States
{
  public class LoadProgressState : IGameState
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadService;

    public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
    {
      _gameStateMachine = gameStateMachine;
      _progressService = progressService;
      _saveLoadService = saveLoadService;
    }

    public void Exit()
    {
      
    }

    public void Enter()
    {
      LoadProgressOrInitNew();
      _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
    }

    private void LoadProgressOrInitNew() =>
      _progressService.Progress =
        _saveLoadService.LoadProgress() 
        ?? NewProgress();

    private PlayerProgress NewProgress()
    {
      var progress = new PlayerProgress(initialLevel: "Main");
      progress.HeroState.MaxHP = 50;
      progress.HeroState.ResetHP();
      return progress;
    }
  }
}