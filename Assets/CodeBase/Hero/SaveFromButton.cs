using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Hero
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
        SaveProgress();

    }

    public void SaveProgress() => 
      _saveLoadService.SaveProgress();
  }
}