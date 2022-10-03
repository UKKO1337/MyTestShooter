using CodeBase.Hero;
using CodeBase.Hero.PlayerController;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI
{
  public class ActorUI : MonoBehaviour
  {
    [SerializeField] private HPBar _hpBar;
    [SerializeField] private HitMarkOnPlayer _hitMark;
    [SerializeField] private SprintBar _sprintBar;

    private IHealth _playerHealth;
    private PlayerMover _playerMover;
    private bool _isDead;
    

    private void OnDestroy() => 
      _playerHealth.HealthChanged -= UpdateHPBar;

    
    public void Construct(IHealth health, PlayerMover mover)
    {
      _playerHealth = health;
      _playerHealth.HealthChanged += UpdateHPBar;
      _playerHealth.HealthChanged += PlayDeath;
      _playerHealth.TookDamage += ShowMark;
      _playerMover = mover;
      _playerMover.OnPlayerSprintStart += UpdateStaminaBar;
      _playerMover.OnPlayerSprintEnd += UpdateStaminaBar;
    }

    private void PlayDeath()
    {
      if (!_isDead && _playerHealth.Current <= 0)
        Die();
    }
    private void Die()
    {
      _isDead = true;
      _hitMark.ShowDeath();
    }

    private void UpdateHPBar() => 
      _hpBar.SetValue(_playerHealth.Current, _playerHealth.Max);

    private void UpdateStaminaBar() => 
      _sprintBar.TransformSprintBar();
    

    private void ShowMark() => 
      _hitMark.ShowHit();
  }
}