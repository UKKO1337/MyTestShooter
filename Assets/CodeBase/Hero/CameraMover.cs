using CodeBase.Infrastructure;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
   public class CameraMover : MonoBehaviour
   {
      [SerializeField] private float _rotateSpeed;
      [SerializeField] private Transform _player;

      private float _xRotation = 0f;
      private IInputService _inputService;

      private void Awake()
      {
         Cursor.lockState = CursorLockMode.Locked;
         _inputService = Game.InputService;
      }
   
      private void OnEnable()
      {
         _inputService.Enable();
      }

      private void OnDisable()
      {
         _inputService.Disable();
      }
      
      private void LateUpdate()
      {
         Look(_inputService.LookAxis);
      }

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
