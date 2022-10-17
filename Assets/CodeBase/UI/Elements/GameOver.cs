using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
  public class GameOver : MonoBehaviour, ILevelTransfer
  {
    [SerializeField] private GameObject _gameOver;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _exitButton;


    private KillCounter _killCounter;
    private IGameStateMachine _stateMachine;

    public void Construct(KillCounter killCounter)
    {
      _gameOver.SetActive(false);
      _stateMachine = AllServices.Container.Single<IGameStateMachine>();
      _killCounter = killCounter;
      _killCounter.MissionAccomplished += ShowGameOver;
      _newGameButton.onClick.AddListener(StartNewGame);
      _exitButton.onClick.AddListener(ExitGame);
    }
    

    private void ShowGameOver()
    {
      _gameOver.SetActive(true);
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