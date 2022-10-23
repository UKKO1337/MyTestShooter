using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;
using Zenject;

namespace CodeBase.Infrastructure.States
{
  public class GameStateMachine : IGameStateMachine
  {
    private Dictionary<Type, IGameBaseState> _states;
    private IGameBaseState _activeState;

    private BootstrapState _bootstrapState;
    private MainMenuState _mainMenuState;
    private LoadLevelState _loadLevelState;
    private LoadProgressState _loadProgressState;
    private GameLoopState _gameLoopState;


    [Inject]
    private void Construct(BootstrapState bootstrapState, MainMenuState mainMenuState,
      LoadLevelState loadLevelState, LoadProgressState loadProgressState, GameLoopState gameLoopState)
    {
      _bootstrapState = bootstrapState;
      _mainMenuState = mainMenuState;
      _loadLevelState = loadLevelState;
      _loadProgressState = loadProgressState;
      _gameLoopState = gameLoopState;
      _states = new Dictionary<Type, IGameBaseState>
      {
        [typeof(BootstrapState)] = _bootstrapState,
        [typeof(MainMenuState)] = _mainMenuState,
        [typeof(LoadLevelState)] = _loadLevelState,
        [typeof(LoadProgressState)] = _loadProgressState,
        [typeof(GameLoopState)] = _gameLoopState,
        
      };
    }
    
    public GameStateMachine()
    {
      
    }
    
    
    public void Enter<TState>() where TState : class, IGameState
    {
      IGameState state = ChangeState<TState>();
      state.Enter();
    }
    

    public void Enter<TState, TParameter>(TParameter payload) where TState : class, IGamePayloadedState<TParameter>
    {
      TState state = ChangeState<TState>();
      state.Enter(payload);
    }

    private TState ChangeState<TState>() where TState : class, IGameBaseState
    {
      _activeState?.Exit();
      
      TState state = GetState<TState>();
      _activeState = state;
      
      return state;
    }

    private TState GetState<TState>() where TState : class, IGameBaseState => 
      _states[typeof(TState)] as TState;
  }
}