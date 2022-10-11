using System.Collections.Generic;
using System.Threading.Tasks;
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

    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();

    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

    private GameObject HeroGameObject { get; set; }

    private GameObject HUDGameObject { get; set; }


    public GameFactory(IAssets asset, IStaticDataService staticData, IInputService inputService)
    {
      _asset = asset;
      _staticData = staticData;
    }

    public async Task WarmUp()
    {
      await _asset.Load<GameObject>(AssetsAddress.Spawner);
    }

    public async Task<GameObject> CreateHero(Vector3 at)
    {
      HeroGameObject = await InstantiateRegisteredAsync(AssetsAddress.HeroPath, at);
      return HeroGameObject;
    }

    public async Task<GameObject> CreateHUD()
    {
      HUDGameObject = await InstantiateRegisteredAsync(AssetsAddress.HUD);
      return HUDGameObject;
    }

    public async Task<GameObject> CreateZombie(ZombieTypeId typeId, Transform parent)
    {
      ZombieStaticData zombieData = _staticData.ForZombie(typeId);

      GameObject prefab = await _asset.Load<GameObject>(zombieData.prefabReference);
      
      GameObject zombie = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);
      
      IHealth health = zombie.GetComponent<IHealth>();
      health.Current = zombieData.Hp;
      health.Max = zombieData.Hp;
      
      zombie.GetComponent<AgentMoveToPlayer>().Construct(HeroGameObject.transform);
      zombie.GetComponent<NavMeshAgent>().speed = zombieData.MoveSpeed;
      
      Attack attack = zombie.GetComponent<Attack>();
      attack.Construct(HeroGameObject.transform);
      attack.Damage = zombieData.Damage;
      attack.Cleavage = zombieData.Cleavage;
      attack.EffectiveDistance = zombieData.EffectiveDistance;
      
      zombie.GetComponent<RotateToPlayer>()?.Construct(HeroGameObject.transform);

      return zombie;
    }

    public async Task CreateSpawner(Vector3 at, string spawnerId, ZombieTypeId zombieTypeId)
    {
      GameObject prefab = await _asset.Load<GameObject>(AssetsAddress.Spawner);
      SpawnPoint spawner = InstantiateRegistered(prefab, at)
        .GetComponent<SpawnPoint>();
      
      spawner.Construct(this);
      spawner.Id = spawnerId;
      spawner.ZombieTypeId = zombieTypeId;
    }

    private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
    {
      GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 at)
    {
      GameObject gameObject = await _asset.Instantiate(prefabPath, at);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
    {
      GameObject gameObject = await _asset.Instantiate(prefabPath);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    public void Cleanup()
    {
      ProgressReaders.Clear();
      ProgressWriters.Clear();
      
      _asset.CleanUp();
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