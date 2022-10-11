using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.StaticData;

namespace CodeBase.Infrastructure.States
{
  public class BootstrapState : IGameState
  {
    private const string Initial = "Initial";
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly AllServices _services;
    private readonly LoadingCurtain _curtain;

    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services, LoadingCurtain curtain)
    {
      _curtain = curtain;
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _services = services;

      RegisterServices();
      EnableInputService();
    }

    private void EnableInputService() => 
      _services.Single<IInputService>().Enable();

    public void Enter()
    {
      _curtain.Show();
      _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
    }

    public void Exit() => 
      _curtain.Hide();

    private void EnterLoadLevel() =>
      _stateMachine.Enter<MainMenuState>();

    private void RegisterServices()
    {
      RegisterStaticData();
      
      _services.RegisterSingle<IGameStateMachine>(_stateMachine);
      _services.RegisterSingle<IInputService>(new InputService());
      RegisterAssetProvider();
      _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
      _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssets>(),
        _services.Single<IStaticDataService>(), _services.Single<IInputService>()));
      _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>(),
        _services.Single<IGameFactory>()));
    }

    private void RegisterAssetProvider()
    {
      AssetProvider assetProvider = new AssetProvider();
      assetProvider.Initialize();
      _services.RegisterSingle<IAssets>(assetProvider);
    }

    private void RegisterStaticData()
    {
      IStaticDataService staticData = new StaticDataService();
      staticData.LoadZombies();
      _services.RegisterSingle(staticData);
    }
  }
}
