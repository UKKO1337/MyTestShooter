using UnityEngine;
using Zenject;

namespace CodeBase.Installers
{
  public class MainMenuInstaller : MonoInstaller
  {
    [SerializeField] private GameObject _mainMenu;

    public override void InstallBindings()
    {
      InitMainMenu();
    }

    private void InitMainMenu()
    {
      Container.InstantiatePrefab(_mainMenu);
    }
  }
}
