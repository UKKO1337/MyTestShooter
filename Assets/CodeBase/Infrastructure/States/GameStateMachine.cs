﻿using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.SaveLoad;

namespace CodeBase.Infrastructure.States
{
  public class GameStateMachine : IGameStateMachine
  {
    private readonly Dictionary<Type, IGameBaseState> _states;
    private IGameBaseState _activeState;


    public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain curtain, AllServices services)
    {
      _states = new Dictionary<Type, IGameBaseState>
      {
        [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services, curtain),
        [typeof(MainMenuState)] = new MainMenuState (this, sceneLoader, curtain),
        [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, curtain, services.Single<IGameFactory>(), services.Single<IPersistentProgressService>(), services.Single<IStaticDataService>()),
        [typeof(LoadProgressState)] = new LoadProgressState(this,services.Single<IPersistentProgressService>(),services.Single<ISaveLoadService>()),
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