using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Logic;
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
    
    [SerializeField] private Transform _camera;

    private IInputService _inputService;
    private int _layerMask;

    private void Awake()
    {
      _inputService = AllServices.Container.Single<IInputService>();
      _layerMask = 1 << LayerMask.NameToLayer("Hittable");
    }

    private void Update()
    {
      Debug.DrawRay(_camera.position, _camera.forward, Color.red);
      PerformShoot();
    }


    private void PerformShoot()
    {
      if (_inputService.IsShootButtonPressed())
      {
        _muzzleFlash.Play();
        _audioSource.PlayOneShot(_shotFx);
        ShootRecoil();

        RaycastHit hit;
        
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, _range, _layerMask))
        {
          IHealth health = hit.collider.GetComponent<IHealth>();
          
          if (health != null)
          {
            health.TakeDamage(_damage);
            Debug.Log("You hit " + hit.collider.name);
          }
        }
      }
      
    }

    private void ShootRecoil()
    {
      Vector3 recoil = new Vector3(-15f, transform.rotation.y, transform.rotation.z);
      Vector3 noRecoil = new Vector3(0, transform.rotation.y, transform.rotation.z);
      transform.DOLocalRotate(recoil, _fireRate).From(noRecoil).SetLoops(2, LoopType.Yoyo);
    }
  }
}
