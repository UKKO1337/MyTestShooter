using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.UI.Elements;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero.PlayerController
{
  public class PlayerCrouching : MonoBehaviour
  {
    [SerializeField] private PlayerDeath _playerDeath;
    
    private IInputService _inputService;
    private Vector3 _originalScale;
    private float _crouchHeight = 0.75f;


    [Inject]
    private void Construct(IInputService inputService)
    {
      _inputService = inputService;
    }

    private void Awake()
    {
      _originalScale = transform.localScale;
      _playerDeath.Dead += CrouchingOff;
    }

    private void Update()
    {
      Crouch();
    }

    private void Crouch()
    {
      if (_inputService.IsCrouchButtonPressed())
        transform.localScale = new Vector3(_originalScale.x, _crouchHeight, _originalScale.z);

      else
        transform.localScale = new Vector3(_originalScale.x, _originalScale.y, _originalScale.z);
    }

    private void CrouchingOff() => 
      enabled = false;
  }
}