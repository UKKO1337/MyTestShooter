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
    [SerializeField] private TrailRenderer _bulletTrail;
    [SerializeField] private float _bulletSpeed = 100f;
    [SerializeField] private Camera _camera;

    private IInputService _inputService;
    private int _layerMask;


    private void Awake()
    {
      _inputService = AllServices.Container.Single<IInputService>();
      _layerMask = 1 << LayerMask.NameToLayer("Hittable");
    }

    private void Update() => 
      PerformShoot();

    public void DeathAnimation()
    {
      Vector3 hideWeapon = new Vector3(90f, transform.rotation.y, transform.rotation.z);
      transform.DOLocalRotate(hideWeapon, 2);
    }


    private void PerformShoot()
    {
      if (_inputService.IsShootButtonPressed())
      {
        _muzzleFlash.Play();
        _audioSource.PlayOneShot(_shotFx);
        ShootRecoil();

        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _range, _layerMask))
        {
          IHealth health = hit.collider.GetComponent<IHealth>();
          
          if (health != null) 
            health.TakeDamage(_damage);
          
          TrailRenderer trail = Instantiate(_bulletTrail, _bulletSpawnPosition.position, Quaternion.identity);
          StartCoroutine(SpawnTrail(trail: trail, point: hit.point));
        }

        else 
        {
          TrailRenderer trail = Instantiate(_bulletTrail, _bulletSpawnPosition.position, Quaternion.identity);
          StartCoroutine(SpawnTrail(trail: trail, point: _camera.transform.forward * 100f));
        }
        
        
      }
      
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 point)
    {
      Vector3 startPosition = trail.transform.position;
      float distance = Vector3.Distance(trail.transform.position, point);
      float remainingDistance = distance;

      while (remainingDistance > 0)
      {
        trail.transform.position = Vector3.Lerp(startPosition, point, 1 - (remainingDistance / distance));

        remainingDistance -= _bulletSpeed * Time.deltaTime;

        yield return null;
      }
      
      trail.transform.position = point;

      Destroy(trail.gameObject, trail.time);
    }

    private void ShootRecoil()
    {
      Vector3 recoil = new Vector3(-15f, transform.rotation.y, transform.rotation.z);
      Vector3 noRecoil = new Vector3(0, transform.rotation.y, transform.rotation.z);
      transform.DOLocalRotate(recoil, _fireRate).From(noRecoil).SetLoops(2, LoopType.Yoyo);
    }
  }
}
