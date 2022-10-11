using CodeBase.Infrastructure.States;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Logic
{
  public class LevelTransfer : MonoBehaviour
  {

    private IGameStateMachine _stateMachine;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;
    private bool _IsNewGame;


    private void Awake()
    {
      _stateMachine = AllServices.Container.Single<IGameStateMachine>();
      _newGameButton.onClick.AddListener(ContinueGame);
      _continueButton.onClick.AddListener(StartNewGame);
      _exitButton.onClick.AddListener(ExitGame);
      
    }

    private void StartNewGame()
    {
      _IsNewGame = true;
      _stateMachine.Enter<LoadProgressState, bool>(_IsNewGame);
    }

    private void ContinueGame()
    {
      _IsNewGame = false;
      _stateMachine.Enter<LoadProgressState, bool>(_IsNewGame);
    }

    private void ExitGame()
    {
      Application.Quit();
      Debug.Log("Ты вышел из игры");
    }
  }
}