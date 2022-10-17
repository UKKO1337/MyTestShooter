using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class EnemyDeath : MonoBehaviour
  {
    [SerializeField] private EnemyHealth _enemyHealth;
    [SerializeField] private EnemyAnimator _enemyAnimator;
    [SerializeField] private Collider _enemyCollider;

    public event Action Happend;

    private void Start() => 
      _enemyHealth.HealthChanged += HealthChanged;

    private void OnDestroy() => 
      _enemyHealth.HealthChanged -= HealthChanged;


    private void HealthChanged()
    {
      if (_enemyHealth.Current <= 0)
        Die();
    }

    private void Die()
    {
      Happend?.Invoke();
      _enemyHealth.HealthChanged -= HealthChanged;
      _enemyAnimator.PlayDeath();
      _enemyCollider.enabled = false;

      StartCoroutine(DestroyTimer());
    }

    private IEnumerator DestroyTimer()
    {
      yield return new WaitForSeconds(3);
      Destroy(gameObject);
    }
  }
}