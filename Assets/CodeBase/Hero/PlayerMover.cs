using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
 [RequireComponent(typeof(Rigidbody))]

 public class PlayerMover : MonoBehaviour
 {
  [SerializeField] private float _sprintSpeed;
  [SerializeField] private float _speed;
  [SerializeField] private float _jumpForce;
  
  
  private Rigidbody _rigidbody;
  private bool _isGrounded;
  private IInputService _inputService;
  

  private void Awake()
  {
   _inputService = AllServices.Container.Single<IInputService>();
  }

  private void Start()
  {
   _rigidbody = GetComponent<Rigidbody>();
  }

  private void OnEnable()
  {
   _inputService.Enable();
  }
  private void OnDisable()
  {
   _inputService.Disable();
  }

  private void FixedUpdate()
  {
   Move(_inputService.MoveAxis);
   Jump();
  }

  private void Move(Vector2 direction)
  {
   Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
   
   if (_inputService.IsSprintButtonPressed())
    SprintMove(moveDirection);
   else
    _rigidbody.AddRelativeForce(moveDirection * _speed);
  }

  private void SprintMove(Vector3 moveDirection)
  {
   _rigidbody.AddRelativeForce(moveDirection * _sprintSpeed);
  }

  private void Jump()
  {
   if (_isGrounded && _inputService.IsJumpButtonPressed())
    _rigidbody.AddForce(Vector3.up * _jumpForce);
  }
  

  private void OnCollisionEnter(Collision collision)
  {
   IsGroundedUpdate(collision, true);
  }

  void OnCollisionExit(Collision collision)
  {
   IsGroundedUpdate(collision, false);
  }

  private void IsGroundedUpdate(Collision collision, bool value)
  {
   if (collision.gameObject.CompareTag(("Ground")))
    _isGrounded = value;
  }
 }
}
