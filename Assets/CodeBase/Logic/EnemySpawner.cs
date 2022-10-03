using System;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic
{
  public class EnemySpawner : MonoBehaviour, ISavedProgress
  {
    [SerializeField] private ZombieTypeId _zombieTypeId;
    [SerializeField] private bool _slain;
    private string _id;
    private IGameFactory _factory;
    private EnemyDeath _enemyDeath;

    private void Awake()
    {
      _id = GetComponent<UniqueId>().Id;
      _factory = AllServices.Container.Single<IGameFactory>();
    }

    public void LoadProgress(PlayerProgress progress)
    {
      if (progress.KillData.ClearedSpawners.Contains(_id))
        _slain = true;
      else
        Spawn();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      if(_slain)
        progress.KillData.ClearedSpawners.Add(_id);
      
    }

    private void Spawn()
    {
      GameObject zombie = _factory.CreateZombie(_zombieTypeId, transform);
      _enemyDeath = zombie.GetComponent<EnemyDeath>();
      _enemyDeath.Happend += Slay;
    }

    private void Slay()
    {
      if (_enemyDeath != null) 
        _enemyDeath.Happend -= Slay;
      
      _slain = true;
    }
  }
}