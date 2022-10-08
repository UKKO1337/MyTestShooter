using System;
using CodeBase.Services;
using CodeBase.Services.Input;
using UnityEngine;
using DG.Tweening;

namespace CodeBase.Hero.PlayerController
{
   public class CameraMover : MonoBehaviour
   {
      [SerializeField] private float _rotateSpeed;
      [SerializeField] private Transform _player;
      [SerializeField] private Camera _camera;
      [SerializeField] [Range(60f, 100f)] private float _fov;
      [SerializeField] private float _zoomFov = 30f;


      private PlayerMover _playerMover;
      private float _zoomStepTime = 10f;
      private float _sprintFov = 120f;

      private float _sprintZoomStepTime = 10f;

      private float _xRotation;

      private IInputService _inputService;


      private void Awake()
      {
         _inputService = AllServices.Container.Single<IInputService>();
         Cursor.lockState = CursorLockMode.Locked;
         _playerMover = GetComponentInParent<PlayerMover>();
         _camera.fieldOfView = _fov;
      }

      private void Start()
      {
         _playerMover.OnPlayerSprintStart += SprintFovOn;
         _playerMover.OnPlayerSprintEnd += SprintFovOff;
      }

      private void OnDestroy()
      {
         _playerMover.OnPlayerSprintStart -= SprintFovOn;
         _playerMover.OnPlayerSprintEnd -= SprintFovOff;
      }


      private void LateUpdate()
      {
         Look(_inputService.LookAxis);
         Zoom(_inputService.IsZoomButtonPressed());
      }

      public void DeathAnimation()
      {
         Vector3 deathPosition = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
         transform.DOMove(endValue: deathPosition, duration: 2f);
         Vector3 deathRotation = new Vector3(_player.rotation.x, _player.rotation.y, 80f);
         _player.DORotate(deathRotation, 2f);
      }

      private void SprintFovOn() =>
         AdjustFieldOfView(_sprintFov, _sprintZoomStepTime);

      private void SprintFovOff() =>
         AdjustFieldOfView(_fov, _sprintZoomStepTime);

      private void Zoom(bool isZoomButtonPressed)
      {
         if (isZoomButtonPressed)
            AdjustFieldOfView(_zoomFov, _zoomStepTime);
         else 
            AdjustFieldOfView(_fov, _zoomStepTime);
      }

      private void AdjustFieldOfView(float fov, float stepTime) => 
         _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, fov, stepTime * Time.deltaTime);

      private void Look(Vector2 lookDirection)
      {
         float mouseAxisY = lookDirection.y * _rotateSpeed * Time.deltaTime;
         float mouseAxisX = lookDirection.x * _rotateSpeed * Time.deltaTime;

         _xRotation -= mouseAxisY;
         _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
         
         _player.Rotate(mouseAxisX * new Vector3(0, 1, 0));
         transform.localRotation = Quaternion.Euler(_xRotation,0,0);
      }
   }
}
