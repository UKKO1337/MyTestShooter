using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
      public LoadingCurtain Curtain;
      
      public Game game;

      private void Awake()
      {
        game = new Game(this, Curtain);
        game.StateMachine.Enter<BootstrapState>();
        
        DontDestroyOnLoad(this);
      }
    }
}
