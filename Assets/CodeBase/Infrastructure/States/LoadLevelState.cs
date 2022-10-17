using System.Threading.Tasks;
using CodeBase.Hero;
using CodeBase.Hero.PlayerController;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using CodeBase.UI;
using CodeBase.UI.Elements;
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
      _gameFactory.WarmUp();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() => 
      _curtain.Hide();

    private async void OnLoaded()
    {
      await InitGameWorld();
      InformProgressReaders();
      
      _stateMachine.Enter<GameLoopState>();
    }

    private void InformProgressReaders()
    {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
        progressReader.LoadProgress(_progressService.Progress);
    }

    private async Task InitGameWorld()
    {
      LevelStaticData levelData = LevelStaticData();
      
      await InitSpawners(levelData);
      GameObject hero = await InitHero(levelData);
      GameObject hud = await InitHud(hero, levelData);
      await InitUI(hud, hero);
    }

    private async Task InitUI(GameObject hud, GameObject hero)
    {
      GameObject ui = await _gameFactory.CreateUI();
      ui.GetComponentInChildren<GameOver>().Construct(hud.GetComponentInChildren<KillCounter>());
      ui.GetComponentInChildren<GameOverDead>().Construct(hero.GetComponentInChildren<PlayerDeath>());

    }

    private async Task InitSpawners(LevelStaticData levelData)
    {
      foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
        await _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.ZombieTypeId);
    }

    private async Task<GameObject> InitHud(GameObject hero, LevelStaticData levelData)
    {
      GameObject hud = await _gameFactory.CreateHUD();
      
      hud.GetComponentInChildren<ActorUI>()
        .Construct(hero.GetComponent<PlayerHealth>(), hero.GetComponent<PlayerMover>());
      hud.GetComponentInChildren<KillCounter>().Construct(levelData);
      
      return hud;
    }

    private async Task<GameObject> InitHero(LevelStaticData levelData)
    {
      GameObject hero = await _gameFactory.CreateHero(at: levelData.InitialPlayerPosition);
      return hero;
    }

    private LevelStaticData LevelStaticData() => 
      _staticData.ForLevel(SceneManager.GetActiveScene().name);
  }
}