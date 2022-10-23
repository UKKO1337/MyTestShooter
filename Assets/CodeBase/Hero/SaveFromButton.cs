using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
  public class SaveFromButton : MonoBehaviour, ISave
  {
    private IInputService _inputService;
    private ISaveLoadService _saveLoadService;

    
    
    
    [Inject]
    private void Construct(IInputService inputService, ISaveLoadService saveLoadService)
    {
      _saveLoadService = saveLoadService;
      _inputService = inputService;
    }
    

    private void Update()
    {
      if (_inputService.IsSaveButtonPressed()) 
        SaveProgress();

    }

    public void SaveProgress() => 
      _saveLoadService.SaveProgress();
  }
}