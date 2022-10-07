using CodeBase.Hero;
using CodeBase.Hero.PlayerController;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
  public class LoadLevelState : IGamePayloadedState<string>
  {
    private const string EnemySpawnerTag = "EnemySpawner";
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;

    public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain, IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticData)
    {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _curtain = curtain;
      _gameFactory = gameFactory;
      _progressService = progressService;
      _staticData = staticData;
    }

    public void Enter(string sceneName)
    {
      _curtain.Show();
      _gameFactory.Cleanup();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() => 
      _curtain.Hide();

    private void OnLoaded()
    {
      InitGameWorld();
      InformProgressReaders();
      
      _stateMachine.Enter<GameLoopState>();
    }

    private void InformProgressReaders()
    {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
        progressReader.LoadProgress(_progressService.Progress);
    }

    private void InitGameWorld()
    {
      LevelStaticData levelData = LevelStaticData();
      
      InitSpawners(levelData);
      GameObject hero = InitHero(levelData);
      InitHud(hero);
    }

    private void InitSpawners(LevelStaticData levelData)
    {
      foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
        _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.ZombieTypeId);
    }

    private void InitHud(GameObject hero)
    {
      GameObject hud = _gameFactory.CreateHUD();
      
      hud.GetComponentInChildren<ActorUI>()
        .Construct(hero.GetComponent<PlayerHealth>(), hero.GetComponent<PlayerMover>());
    }

    private GameObject InitHero(LevelStaticData levelData) => 
      _gameFactory.CreateHero(at: levelData.InitialPlayerPosition);

    private LevelStaticData LevelStaticData() => 
      _staticData.ForLevel(SceneManager.GetActiveScene().name);
  }
}