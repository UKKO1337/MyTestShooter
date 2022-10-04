using System.Collections.Generic;
using System.Linq;
using CodeBase.Logic;
using CodeBase.Services;

namespace CodeBase.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private Dictionary<ZombieTypeId, ZombieStaticData> _monsters;
    private Dictionary<string, LevelStaticData> _levels;

    public void LoadZombies()
    {
      _monsters = UnityEngine.Resources
        .LoadAll<ZombieStaticData>("StaticData/Zombie")
        .ToDictionary(x => x.ZombieTypeId, x => x);
      _levels = UnityEngine.Resources
        .LoadAll<LevelStaticData>("StaticData/Levels")
        .ToDictionary(x => x.LevelKey, x => x);
    }

    public ZombieStaticData ForZombie(ZombieTypeId typeId) => 
      _monsters.TryGetValue(typeId, out ZombieStaticData staticData) 
        ? staticData 
        : null;

    public LevelStaticData ForLevel(string sceneKey) =>
      _levels.TryGetValue(sceneKey, out LevelStaticData staticData) 
      ? staticData 
      : null;
    
  }
}