using System;
using CodeBase.Data;
using CodeBase.Services.PersistentProgress;
using CodeBase.StaticData;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Elements
{
  public class KillCounter : MonoBehaviour, ISavedProgress
  {
    public TextMeshProUGUI Counter;
    private KillData _killData;
    private LevelStaticData _staticData;
    public event Action MissionAccomplished;
    private int Spawners => _killData.Spawners;

    public void Construct(LevelStaticData staticData) => 
      _staticData = staticData;

    private void UpdateCounter()
    {
      Counter.text = $"{Spawners} / {_staticData.EnemySpawners.Capacity}";

      if (Spawners >= _staticData.EnemySpawners.Capacity) 
        MissionAccomplished?.Invoke();
    }

    public void LoadProgress(PlayerProgress progress)
    {
      _killData = progress.KillData;
      UpdateCounter();
      _killData.Happend += UpdateCounter;
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      progress.KillData = _killData;
    }
  }
}