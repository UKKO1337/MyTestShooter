using CodeBase.Logic;
using Resources.StaticData;

namespace CodeBase.Services
{
  public interface IStaticDataService : IService
  {
    void LoadZombies();
    ZombieStaticData ForZombie(ZombieTypeId typeId);
  }
}