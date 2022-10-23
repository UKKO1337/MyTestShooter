using CodeBase.Infrastructure;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using CodeBase.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers
{
  public class ServiceInstallers : MonoInstaller
  {
    [SerializeField] private LoadingCurtain _curtain;

    public override void InstallBindings()
    {
      BindInputService();
      BindGame();
      BindCoroutine();
      BindStates();
      BindStateMachine();
      BindSceneLoader();
      BindAssetProvider();
      BindStaticDataService();
      BindProgressService();
      BindGameFactory();
      BindSaveLoadService();
      BindLoadingCurtain();
    }

    private void BindSaveLoadService()
    {
      Container
        .Bind<ISaveLoadService>()
        .To<SaveLoadService>()
        .FromNew()
        .AsSingle();
    }

    private void BindGameFactory()
    {
      Container
        .Bind<IGameFactory>()
        .To<GameFactory>()
        .FromNew()
        .AsSingle();
    }

    private void BindProgressService()
    {
      Container
        .Bind<IPersistentProgressService>()
        .To<PersistentProgressService>()
        .FromNew()
        .AsSingle();
    }

    private void BindStaticDataService()
    {
      Container
        .Bind<IStaticDataService>()
        .To<StaticDataService>()
        .FromNew()
        .AsSingle();
    }

    private void BindAssetProvider()
    {
      Container
        .Bind<IAssets>()
        .To<AssetProvider>()
        .FromNew()
        .AsSingle();
    }

    private void BindSceneLoader()
    {
      Container
        .Bind<SceneLoader>()
        .AsSingle();
    }

    private void BindStateMachine()
    {
      Container
        .Bind<IGameStateMachine>()
        .To<GameStateMachine>()
        .FromNew()
        .AsSingle();
    }

    private void BindStates()
    {
      Container
        .Bind<BootstrapState>()
        .AsSingle()
        .NonLazy();
      Container
        .Bind<MainMenuState>()
        .AsSingle()
        .NonLazy();
      Container
        .Bind<LoadProgressState>()
        .AsSingle()
        .NonLazy();
      Container
        .Bind<LoadLevelState>()
        .AsSingle()
        .NonLazy();
      Container
        .Bind<GameLoopState>()
        .AsSingle()
        .NonLazy();
    }

    private void BindLoadingCurtain()
    {
      LoadingCurtain curtain = Container.InstantiatePrefabForComponent<LoadingCurtain>(_curtain);

      Container
        .Bind<LoadingCurtain>()
        .To<LoadingCurtain>()
        .FromInstance(curtain)
        .AsSingle();
    }

    private void BindCoroutine()
    {
      Container
        .Bind<ICoroutineRunner>()
        .To<CoroutineRunner>()
        .FromMethod(FindObjectOfType<CoroutineRunner>)
        .AsSingle();
    }

    private void BindGame()
    {
      Container
        .Bind<Game>()
        .AsSingle();
    }

    private void BindInputService()
    {
      Container
        .Bind<IInputService>()
        .To<InputService>()
        .FromInstance(new InputService())
        .AsSingle();
    }
  }
}