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
    public GameObject CreateHero(GameObject at);
    public GameObject CreateHUD();
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    
    void Cleanup();
    void Register(ISavedProgressReader progressReader);
    GameObject CreateZombie(ZombieTypeId typeId, Transform parent);
  }
}