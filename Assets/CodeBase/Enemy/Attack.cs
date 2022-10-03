using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
  [RequireComponent(typeof(EnemyAnimator))]
  public class Attack : MonoBehaviour
  {
    public float Damage = 10f;
    public EnemyAnimator Animator;
    public float AttackCooldown = 3f;
    public float Cleavage = 0.5f;
    public float EffectiveDistance = 0.5f;

    private Transform _heroTransform;
    private float _attackCooldown;

    private bool _isAttacking;

    private int _layerMask;

    private Collider[] _hits = new Collider[1];

    private bool _attackIsActive;


    public void Construct(Transform heroTransform) => 
      _heroTransform = heroTransform;

    private void Awake() => 
      _layerMask = 1 << LayerMask.NameToLayer("Player");

    private void Update()
    {
      UpdateCooldown();
      
      if (CanAttack()) 
        StartAttack();
      else if (_isAttacking) 
        OnAttackEnded();

    }

    private void OnAttack()
    {
      if (Hit(out Collider hit))
      {
        PhysicsDebug.DrawDebug(StartPoint(), Cleavage, 1 );
        hit.transform.GetComponent<IHealth>().TakeDamage(Damage);
      }
      
    }

    private void OnAttackEnded()
    {
      _attackCooldown = AttackCooldown;
      _isAttacking = false;
    }

    public void EnableAttack() => 
      _attackIsActive = true;

    public void DisableAttack() => 
      _attackIsActive = false;

    private bool Hit(out Collider hit)
    {
      var hitsCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);

      hit = _hits.FirstOrDefault();
      
      return hitsCount > 0;
    }

    private Vector3 StartPoint() => 
      new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * EffectiveDistance;

    private void UpdateCooldown()
    {
      if (_attackCooldown > 0)
        _attackCooldown -= Time.deltaTime;
      
    }

    private void StartAttack()
    {
      transform.LookAt(_heroTransform);
      Animator.PlayAttack();

      _isAttacking = true;
    }

    private bool CanAttack() => 
      _attackIsActive && !_isAttacking && _attackCooldown <= 0;

  }
}
