using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.StaticData
{
  [Serializable]
  public class EnemySpawnerData
  {
    public string Id;
    public ZombieTypeId ZombieTypeId;
    public Vector3 Position;

    public EnemySpawnerData(string id, ZombieTypeId zombieTypeId, Vector3 position)
    {
      Id = id;
      ZombieTypeId = zombieTypeId;
      Position = position;
    }
  }
}