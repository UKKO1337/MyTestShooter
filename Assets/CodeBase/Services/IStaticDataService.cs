using CodeBase.Logic;
using CodeBase.StaticData;

namespace CodeBase.Services
{
  public interface IStaticDataService : IService
  {
    void LoadZombies();
    ZombieStaticData ForZombie(ZombieTypeId typeId);
    LevelStaticData ForLevel(string sceneKey);
  }
}