using System;
using System.Collections.Generic;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
  public interface IGameFactory : IService
  {
    public GameObject CreateHero(Vector3 at);
    public GameObject CreateHUD();
    GameObject CreateZombie(ZombieTypeId typeId, Transform parent);
    void CreateSpawner(Vector3 at, string spawnerId, ZombieTypeId zombieTypeId);
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }

    void Cleanup();
  }
}