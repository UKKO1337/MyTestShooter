using System;
using System.Collections.Generic;
using CodeBase.Logic;
using Unity.VisualScripting;

namespace CodeBase.Infrastructure
{
  public class GameStateMachine
  {
    private readonly Dictionary<Type, IGameBaseState> _states;
    private IGameBaseState _activeState;


    public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain curtain)
    {
      _states = new Dictionary<Type, IGameBaseState>
      {
        [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
        [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, curtain),
        [typeof(GameLoopState)] = new GameLoopState(this),
        
      };
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