using System;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
  public class SpawnPoint : MonoBehaviour, ISavedProgress
  {
    public ZombieTypeId ZombieTypeId;
    private bool _slain;
    public string Id { get; set; }

    private IGameFactory _gameFactory;
    private EnemyDeath _enemyDeath;

    public void Construct(IGameFactory factory) =>
      _gameFactory = factory;

    public void LoadProgress(PlayerProgress progress)
    {
      if (progress.KillData.ClearedSpawners.Contains(Id))
        _slain = true;
      else
        Spawn();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      if (_slain) 
        progress.KillData.ClearedSpawners.Add(Id);

    }

    private async void Spawn()
    {
      GameObject zombie = await _gameFactory.CreateZombie(ZombieTypeId, transform);
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