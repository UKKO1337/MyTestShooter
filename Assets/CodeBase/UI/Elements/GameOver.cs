using System.Collections;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Elements
{
  public class GameOver : MonoBehaviour, ILevelTransfer
  {
    [SerializeField] private GameObject _gameOver;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _exitButton;


    private KillCounter _killCounter;
    private IGameStateMachine _stateMachine;

    [Inject]
    private void Construct(IGameStateMachine stateMachine) => 
      _stateMachine = stateMachine;

    public void Construct(KillCounter killCounter)
    {
      _gameOver.SetActive(false);
      _killCounter = killCounter;
      _killCounter.MissionAccomplished += ShowGameOver;
      _newGameButton.onClick.AddListener(StartNewGame);
      _exitButton.onClick.AddListener(ExitGame);
    }
    

    private void ShowGameOver()
    {
      _gameOver.SetActive(true);
      StartCoroutine(GameOverTimer());
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

    private IEnumerator GameOverTimer()
    {
      yield return new WaitForSeconds(1);
      Cursor.lockState = CursorLockMode.Confined;
      Time.timeScale = 0;
    }
  }
}