using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Hero.PlayerController;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    private readonly IAssets _asset;
    private readonly IStaticDataService _staticData;
    private readonly IInputService _inputService;

    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();

    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

    private GameObject HeroGameObject { get; set; }

    private GameObject HUDGameObject { get; set; }


    public GameFactory(IAssets asset, IStaticDataService staticData, IInputService inputService)
    {
      _inputService = inputService; 
      _asset = asset;
      _staticData = staticData;
    }

    public GameObject CreateHero(Vector3 at)
    {
      HeroGameObject = InstantiateRegistered(AssetPath.HeroPath, at);
      
      var playerMover = HeroGameObject.GetComponent<PlayerMover>();
      playerMover.Construct(_inputService);
      
      HeroGameObject.GetComponent<PlayerJumper>().Construct(_inputService);
      HeroGameObject.GetComponent<PlayerCrouching>().Construct(_inputService);
      
      HeroGameObject.GetComponentInChildren<CameraMover>().Construct(_inputService, playerMover);
      HeroGameObject.GetComponentInChildren<Shoot>().Construct(_inputService);
      
      return HeroGameObject;
    }

    public GameObject CreateHUD()
    {
      HUDGameObject = InstantiateRegistered(AssetPath.HUD);
      return HUDGameObject;
    }

    public GameObject CreateZombie(ZombieTypeId typeId, Transform parent)
    {
      ZombieStaticData zombieData = _staticData.ForZombie(typeId);
      GameObject zombie = Object.Instantiate(zombieData.Prefab, parent.position, Quaternion.identity, parent);
      
      var health = zombie.GetComponent<IHealth>();
      health.Current = zombieData.Hp;
      health.Max = zombieData.Hp;
      
      zombie.GetComponent<AgentMoveToPlayer>().Construct(HeroGameObject.transform);
      zombie.GetComponent<NavMeshAgent>().speed = zombieData.MoveSpeed;
      
      var attack = zombie.GetComponent<Attack>();
      attack.Construct(HeroGameObject.transform);
      attack.Damage = zombieData.Damage;
      attack.Cleavage = zombieData.Cleavage;
      attack.EffectiveDistance = zombieData.EffectiveDistance;
      
      zombie.GetComponent<RotateToPlayer>()?.Construct(HeroGameObject.transform);

      return zombie;
    }

    public void CreateSpawner(Vector3 at, string spawnerId, ZombieTypeId zombieTypeId)
    {
      SpawnPoint spawner = InstantiateRegistered(AssetPath.Spawner, at)
        .GetComponent<SpawnPoint>();
      
      spawner.Construct(this);
      spawner.Id = spawnerId;
      spawner.ZombieTypeId = zombieTypeId;
    }

    private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
    {
      GameObject gameObject = _asset.Instantiate(prefabPath, at);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    private GameObject InstantiateRegistered(string prefabPath)
    {
      GameObject gameObject = _asset.Instantiate(prefabPath);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    public void Cleanup()
    {
      ProgressReaders.Clear();
      ProgressWriters.Clear();
    }

    private void RegisterProgressWatchers(GameObject gameObject)
    {
      foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
        Register(progressReader);
    }

    public void Register(ISavedProgressReader progressReader)
    {
      if (progressReader is ISavedProgress progressWriter)
        ProgressWriters.Add(progressWriter);
      
      ProgressReaders.Add(progressReader);
    }
  }
}