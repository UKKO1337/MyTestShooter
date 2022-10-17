using System;
using CodeBase.Hero.PlayerController;
using UnityEngine;

namespace CodeBase.Hero
{
  [RequireComponent(typeof(PlayerHealth))]
  public class PlayerDeath : MonoBehaviour
  {
    [SerializeField] private PlayerHealth _health;
    
    public event Action Dead;
    
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
      Dead?.Invoke();
      _isDead = true;
    }
  }
}