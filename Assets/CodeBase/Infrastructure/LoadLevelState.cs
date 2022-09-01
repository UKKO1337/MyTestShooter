using System;
using CodeBase.Logic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure
{
  public class LoadLevelState : IGamePayloadedState<string>
  {
    private const string HeroPath = "Hero/Hero";
    private const string InitialPointTag = "InitialPoint";
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;

    public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain)
    {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _curtain = curtain;
    }

    public void Enter(string sceneName)
    {
      _curtain.Show();
      _sceneLoader.Load(sceneName, onLoaded);
    }

    public void Exit() => 
      _curtain.Hide();

    private void onLoaded()
    {
      GameObject initialPoint = GameObject.FindWithTag(InitialPointTag);
      GameObject hero = Instantiate(HeroPath, initialPoint.transform.position);
      _stateMachine.Enter<GameLoopState>();
    }

    private static GameObject Instantiate(string path)
    {
      var prefab = Resources.Load<GameObject>(path);
      return Object.Instantiate(prefab);
    }

    private static GameObject Instantiate(string path, Vector3 at)
    {
      var prefab = Resources.Load<GameObject>(path);
      return Object.Instantiate(prefab, at, Quaternion.identity);
    }
  }
}