using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public interface IGameFactory : IService
  {
    public Task<GameObject> CreateHero(Vector3 at);
    public Task<GameObject> CreateHUD();
    Task<GameObject> CreateZombie(ZombieTypeId typeId, Transform parent);
    Task CreateSpawner(Vector3 at, string spawnerId, ZombieTypeId zombieTypeId);
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }

    void Cleanup();
    Task WarmUp();
    Task<GameObject> CreateUI();
  }
}