using CodeBase.Infrastructure;
using CodeBase.Infrastructure.States;
using Zenject;

namespace CodeBase.Installers
{
  public class GameBootstrapperInstaller : MonoInstaller

  {
    private Game _game;


    [Inject]
    private void Construct(Game game)
    {
      _game = game;
    }
    public override void InstallBindings()
    {
      InitGame();
    }

    private void InitGame()
    {
      _game.StateMachine.Enter<BootstrapState>();
    }
  }
}