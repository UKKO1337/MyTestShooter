using System.Threading.Tasks;
using CodeBase.Hero;
using CodeBase.Hero.PlayerController;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Installers
{
  public class InitGameWorld : MonoInstaller
  {
    private IGameFactory _gameFactory;
    private IStaticDataService _staticData;
    private IPersistentProgressService _progressService;

    [Inject]
    private void Construct(IStaticDataService staticData, IPersistentProgressService progressService)
    {
      _staticData = staticData;
      _progressService = progressService;
    }
    
    public override async void InstallBindings()
    {
      BindGameFactory();
      await GameFactoryWarmUp();
      BindSaveLoadService();
      await InitWorld();
      InformProgressReaders();
    }

    private void BindSaveLoadService()
     {
       Container
         .Bind<ISaveLoadService>()
         .To<SaveLoadService>()
         .FromNew()
         .AsSingle();
     }

    private void BindGameFactory()
    {
      Container
        .Bind<IGameFactory>()
        .To<GameFactory>()
        .FromNew()
        .AsSingle();
    }


    private async Task InitWorld()
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


    private void InformProgressReaders()
    {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
        progressReader.LoadProgress(_progressService.Progress);
    }
    

    private async Task GameFactoryWarmUp()
    {
      _gameFactory = Container.Resolve<IGameFactory>();
      _gameFactory.Cleanup();
      await _gameFactory.WarmUp();
    }
  }
    
  
}
