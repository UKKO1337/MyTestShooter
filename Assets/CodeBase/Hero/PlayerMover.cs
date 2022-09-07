using CodeBase.Data;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
 [RequireComponent(typeof(Rigidbody))]

 public class PlayerMover : MonoBehaviour, ISavedProgress
 {
  [SerializeField] private float _sprintSpeed;
  [SerializeField] private float _speed;
  [SerializeField] private float _jumpForce;
  
  
  private Rigidbody _rigidbody;
  private bool _isGrounded;
  private bool _isOnTheWall;
  private IInputService _inputService;
  private Vector3 _onTheGround;


  private void Awake() => 
   _inputService = AllServices.Container.Single<IInputService>();

  private void Start() => 
   _rigidbody = GetComponent<Rigidbody>();

  private void OnEnable() => 
   _inputService.Enable();

  private void OnDisable() => 
   _inputService.Disable();

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

  private void SprintMove(Vector3 moveDirection) => 
   _rigidbody.AddRelativeForce(moveDirection * _sprintSpeed);

  private void Jump()
  {
   if (_isGrounded && _inputService.IsJumpButtonPressed())
    _rigidbody.AddForce(Vector3.up * _jumpForce);
  }
  

  private void OnCollisionEnter(Collision collision)
  {
   IsGroundedUpdate(collision, true);
   IsOnTheWallUpdate(collision,true);
  }

  private void OnCollisionExit(Collision collision)
  {
   IsGroundedUpdate(collision, false);
   IsOnTheWallUpdate(collision, false);
  }

  private void IsOnTheWallUpdate(Collision collision, bool value)
  {
   if (collision.gameObject.CompareTag(("Wall"))) 
    _isOnTheWall = value;
  }

  private void IsGroundedUpdate(Collision collision, bool value)
  {
   if (collision.gameObject.CompareTag(("Ground")))
    _isGrounded = value;
  }

  public void LoadProgress(PlayerProgress progress)
  {
   if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level)
   {
    Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
    if (savedPosition != null) 
     Warp(to: savedPosition);
   }
    
  }

  private void Warp(Vector3Data to) => 
   transform.position = to.AsUnityVector().AddY(transform.position.y + 0.5f);

  public void UpdateProgress(PlayerProgress progress) => 
   progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(),transform.position.AsVectorData());

  private static string CurrentLevel() => 
   SceneManager.GetActiveScene().name;
 }
}
