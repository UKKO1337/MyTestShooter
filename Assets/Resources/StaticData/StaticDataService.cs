using System.Collections.Generic;
using System.Linq;
using CodeBase.Logic;
using CodeBase.Services;
using UnityEngine;

namespace Resources.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private Dictionary<ZombieTypeId, ZombieStaticData> _monsters;

    public void LoadZombies()
    {
      _monsters = UnityEngine.Resources
        .LoadAll<ZombieStaticData>("StaticData/Zombie")
        .ToDictionary(x => x.ZombieTypeId, x => x);
    }

    public ZombieStaticData ForZombie(ZombieTypeId typeId) => 
      _monsters.TryGetValue(typeId, out ZombieStaticData staticData) 
        ? staticData 
        : null;
  }
}