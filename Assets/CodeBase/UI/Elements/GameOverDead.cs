using CodeBase.Hero;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Elements
{
  public class GameOverDead : MonoBehaviour, ILevelTransfer
  {
    [SerializeField] private GameObject _gameOverDead;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _exitButton;


    private KillCounter _killCounter;
    private IGameStateMachine _stateMachine;
    private PlayerDeath _playerDeath;

    [Inject]
    private void Construct(IGameStateMachine stateMachine)
    {
      _stateMachine = stateMachine;
    }
    
    public void Construct(PlayerDeath playerDeath)
    {
      _gameOverDead.SetActive(false);
      _playerDeath = playerDeath;
      _playerDeath.Dead += ShowGameOver;
      _newGameButton.onClick.AddListener(StartNewGame);
      _exitButton.onClick.AddListener(ExitGame);
    }
    

    private void ShowGameOver()
    {
      _gameOverDead.SetActive(true);
      Cursor.lockState = CursorLockMode.Confined;
    }

    public void StartNewGame() => 
      EnterState();

    public void ContinueGame()
    {
    }

    public void ExitGame()
    {
      Application.Quit();
      Debug.Log("Ты вышел из игры");
    }

    private void EnterState() => 
      _stateMachine.Enter<BootstrapState>();
  }
}
