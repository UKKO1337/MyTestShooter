using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using UnityEngine;

namespace CodeBase.Hero.PlayerController
{
  public class PlayerJumper : MonoBehaviour
  {
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _jumpPower;

    private IInputService _inputService;

    private bool _isGrounded;
    

    private void Awake() => 
      _inputService = AllServices.Container.Single<IInputService>();

    private void Update()
    {
      CheckGround();
      Jump();
    }
    
    private void Jump()
    {
      if (_isGrounded && _inputService.IsJumpButtonPressed())
      {
        _rigidbody.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
        _isGrounded = false;
      }
    }

    private void CheckGround()
    {
      Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
      Vector3 direction = transform.TransformDirection(Vector3.down);
      float distance = 0.75f;

      if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
      {
        Debug.DrawRay(origin, direction * distance, Color.red);
        _isGrounded = true;
      }
      else
        _isGrounded = false;
    }
  }
}