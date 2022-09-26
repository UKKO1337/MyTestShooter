using System;
using UnityEngine;
using DG.Tweening;

namespace CodeBase.Hero
{
  public class HitMarkOnPlayer : MonoBehaviour
  {
    [SerializeField] private CanvasGroup _hitMarkOnPlayer;
    
    public void ShowHit()
    {
      _hitMarkOnPlayer.alpha = 0.8f;
      _hitMarkOnPlayer.DOFade(0f, 1f);
    }

    public void ShowDeath()
    {
      _hitMarkOnPlayer.DOFade(1f, 3f);
    }
  }
}
