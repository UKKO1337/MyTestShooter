using System.Collections;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.Services.Input;
using UnityEngine;
using DG.Tweening;

namespace CodeBase.Hero
{
  public class Shoot : MonoBehaviour
  {
    [SerializeField] private Transform _bulletSpawnPosition;
    
    [SerializeField] private float _fireRate = 0.2f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _range = 15f;
    
    [SerializeField] private AudioClip _shotFx;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private Camera _camera;

    [SerializeField] private PlayerDeath _playerDeath;

    private float _lastShootTime;
    private IInputService _inputService;
    private int _layerMask;


    private void Awake()
    {
      _inputService = AllServices.Container.Single<IInputService>();
      _layerMask = 1 << LayerMask.NameToLayer("Hittable");
      _playerDeath.Dead += Death;
    }

    private void Update() => 
      PerformShoot();


    private void PerformShoot()
    {
      if (CanShoot())
      {
        _muzzleFlash.Play();
        _audioSource.PlayOneShot(_shotFx);
        ShootRecoil();

        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _range, _layerMask))
        {
          IHealth health = hit.collider.GetComponent<IHealth>();
          
          if (health != null) 
            health.TakeDamage(_damage);
        }
        
        _lastShootTime = Time.time;
      }
      
    }

    private bool CanShoot() => 
      _inputService.IsShootButtonPressed() && _lastShootTime + _fireRate < Time.time;


    private void ShootRecoil()
    {
      Vector3 verticalRecoil = new Vector3(-10f, 0, 0);
      Vector3 horizontalRecoil = new Vector3(0, 0, -0.04f);
      
      Vector3 noRecoil = new Vector3(0, 0, 0);
      
      transform.DOLocalRotate(verticalRecoil, 0.2f).From(noRecoil).SetLoops(2, LoopType.Yoyo);
      transform.DOLocalMove(horizontalRecoil, 0.1f).From(noRecoil).SetLoops(2, LoopType.Yoyo);
    }

    private void DeathAnimation()
    {
      Vector3 verticalHideWeapon = new Vector3(90f, 0, 0);
      Vector3 horizontalHideWeapon = new Vector3(0, -0.3f, 0);
      
      transform.DOLocalRotate(verticalHideWeapon, 2);
      transform.DOLocalMove(horizontalHideWeapon, 2);
    }

    private void Death()
    {
      DeathAnimation();
      enabled = false;
    }
  }
}
