using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public partial class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
      public LoadingCurtain CurtainPrefab;
      
      public Game game;

      private void Awake()
      {
        game = new Game(this, Instantiate(CurtainPrefab));
        game.StateMachine.Enter<BootstrapState>();
        
        DontDestroyOnLoad(this);
      }
    }
}
