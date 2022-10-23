using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.StaticData;
using Zenject;

namespace CodeBase.Infrastructure.States
{
  public class BootstrapState : IGameState, IGamePayloadedState<bool>
  {
    private const string Initial = "Initial";
    private IGameStateMachine _stateMachine;
    private SceneLoader _sceneLoader;
    private readonly AllServices _services;
    private LoadingCurtain _curtain;
    private IInputService _inputService;
    private IAssets _assets;
    private IStaticDataService _staticData;


    [Inject]
    private void Construct(IGameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain,
      IInputService inputService, IAssets assets, IStaticDataService staticData)
    {
      _curtain = curtain;
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _inputService = inputService;
      _staticData = staticData;
      _assets = assets;
      _assets.Initialize();
      _staticData.LoadZombies();
      EnableInputService();
    }
    
    

    private void EnableInputService() => 
      _inputService.Enable();

    public void Enter()
    {
      _curtain.Show();
      _sceneLoader.Load(Initial, onLoaded: EnterMenu);
    }

    public void Enter(bool payload)
    {
      _curtain.Show();
      _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
    }

    private void EnterLoadLevel() => 
      _stateMachine.Enter<LoadProgressState>();

    public void Exit() => 
      _curtain.Hide();

    private void EnterMenu() =>
      _stateMachine.Enter<MainMenuState>();

    
  }
}
