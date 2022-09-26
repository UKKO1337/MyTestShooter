using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class Aggro : MonoBehaviour
  {
    [SerializeField] private EnemyDeath _death;
    public TriggerObserver TriggerObserver;
    public Follow Follow;
    

    public float Cooldown;
    
    private Coroutine _aggroCoroutine;
    private bool _hasAggroTarget;

    private void Start()
    {
      TriggerObserver.TriggerEnter += TriggerEnter;
      TriggerObserver.TriggerExit += TriggerExit;
      _death.Happend += Die;

      SwitchFollowOff();
    }

    private void OnDestroy()
    {
      _death.Happend -= Die;
    }

    private void TriggerEnter(Collider obj)
    {
      if (!_hasAggroTarget)
      {
        _hasAggroTarget = true;
        StopAggroCoroutine();
        SwitchFollowOn();
      }
        
    }

    private void TriggerExit(Collider obj)
    {
      if (_hasAggroTarget)
      {
        _hasAggroTarget = false;
        _aggroCoroutine = StartCoroutine(SwitchFollowOffAfterCooldown());
      }
    }

    private void StopAggroCoroutine()
    {
      if (_aggroCoroutine != null)
      {
        StopCoroutine(_aggroCoroutine);
        _aggroCoroutine = null;
      }
    }

    private IEnumerator SwitchFollowOffAfterCooldown()
    {
      yield return new WaitForSeconds(Cooldown);
      
      SwitchFollowOff();
    }

    private void SwitchFollowOn() => 
      Follow.enabled = true;

    private void SwitchFollowOff() => 
      Follow.enabled = false;

    private void Die()
    {
      TriggerObserver.TriggerEnter -= TriggerEnter;
      TriggerObserver.TriggerExit -= TriggerExit;
      _hasAggroTarget = true;
      StopAggroCoroutine();
      SwitchFollowOff();
    }
  }
}