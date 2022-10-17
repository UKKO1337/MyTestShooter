using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using UnityEditor;

namespace CodeBase.Infrastructure.States
{
  public class LoadProgressState :  IGamePayloadedState<bool>, IGameState
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

    public void Enter(bool isNewGame)
    {
      if (isNewGame)
        _progressService.Progress = NewProgress();
      else
        LoadProgress();
      
      _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
    }

    public void Exit()
    {
      
    }

    public void Enter()
    {
      LoadProgress();
      _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
    }


    private void LoadProgress() =>
      _progressService.Progress = 
        _saveLoadService.LoadProgress() 
        ?? NewProgress(); 

    private PlayerProgress NewProgress()
    {
      PlayerProgress progress = new PlayerProgress(initialLevel: "Main");
      progress.HeroState.MaxHP = 50;
      progress.HeroState.ResetHP();
      return progress;
    }
  }
}