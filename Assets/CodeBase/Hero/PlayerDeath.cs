using CodeBase.Hero.PlayerController;
using UnityEngine;

namespace CodeBase.Hero
{
  [RequireComponent(typeof(PlayerHealth))]
  public class PlayerDeath : MonoBehaviour
  {
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private Shoot _shoot;
    [SerializeField] private PlayerMover _mover;
    [SerializeField] private PlayerCrouching _crouching;
    [SerializeField] private PlayerJumper _jumper;
    [SerializeField] private CameraMover _camera;

    private bool _isDead;
    

    private void Start() => 
      _health.HealthChanged += HealthChanged;

    private void OnDestroy() => 
      _health.HealthChanged -= HealthChanged;

    private void HealthChanged()
    {
      if (!_isDead && _health.Current <= 0)
        Die();
    }

    private void Die()
    {
      _isDead = true;
      _mover.enabled = false;
      _crouching.enabled = false;
      _jumper.enabled = false;
      _camera.DeathAnimation();
      _camera.enabled = false;
      _shoot.DeathAnimation();
      _shoot.enabled = false;
    }
  }
}