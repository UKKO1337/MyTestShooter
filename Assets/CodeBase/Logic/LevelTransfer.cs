using CodeBase.Infrastructure.States;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Logic
{
  public class LevelTransfer : MonoBehaviour, ILevelTransfer
  {
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;
    
    private IGameStateMachine _stateMachine;
    private bool _IsNewGame;


    private void Awake()
    {
      _stateMachine = AllServices.Container.Single<IGameStateMachine>();
      _newGameButton.onClick.AddListener(StartNewGame);
      _continueButton.onClick.AddListener(ContinueGame);
      _exitButton.onClick.AddListener(ExitGame);
      
    }

    public void StartNewGame()
    {
      _IsNewGame = true;
      EnterState(_IsNewGame);
    }

    public void ContinueGame()
    {
      _IsNewGame = false;
      EnterState(_IsNewGame);
    }

    public void ExitGame()
    {
      Application.Quit();
      Debug.Log("Ты вышел из игры");
    }

    public void EnterState(bool newGame) => 
      _stateMachine.Enter<LoadProgressState, bool>(newGame);
  }
}