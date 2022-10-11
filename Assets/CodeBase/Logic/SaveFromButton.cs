using System;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Logic
{
  public class SaveFromButton : MonoBehaviour, ISave
  {
    private IInputService _inputService;
    private ISaveLoadService _saveLoadService;

    private void Awake()
    {
      _inputService = AllServices.Container.Single<IInputService>();
      _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
    }

    private void Update()
    {
      if (_inputService.IsSaveButtonPressed())
      {
        SaveProgress();
        Debug.Log("Game saved");
      }
      
    }

    public void SaveProgress() => 
      _saveLoadService.SaveProgress();
  }
}