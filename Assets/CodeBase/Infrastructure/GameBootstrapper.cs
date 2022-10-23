using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public partial class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
      public LoadingCurtain CurtainPrefab;
      
      public Game game;

      private IInputService _inputService;

      

      private void Awake()
      {
        game = new Game();
        game.StateMachine.Enter<BootstrapState>();
        
        DontDestroyOnLoad(this);
      }
    }
}
