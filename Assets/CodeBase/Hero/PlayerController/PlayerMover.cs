using System;
using CodeBase.Data;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero.PlayerController
{
 public class PlayerMover : MonoBehaviour, ISavedProgress
 {
  [SerializeField] private Rigidbody _rigidbody;
  [SerializeField] private float _sprintSpeed;
  [SerializeField] private float _speed;

  public Action OnPlayerSprintStart;
  public Action OnPlayerSprintEnd;
  public static float SprintDuration = 5f;
  public static float SprintRemaining;

  private float _maxVelocityChange = 10f;
  private float _sprintCooldownTimer = 0.5f;
  private float _sprintCooldownReset;

  private bool _isSprintCooldown;

  private bool _isSprinting;

  private IInputService _inputService;


  private void Awake()
  {
   SprintRemaining = SprintDuration;
   _sprintCooldownReset = _sprintCooldownTimer;
   _inputService = AllServices.Container.Single<IInputService>();
  }

  private void Update()
  {
   SprintAvailableChecker();
   SprintStartedOrEnded();
  }


  private void FixedUpdate() => 
   Move(_inputService.MoveAxis);

  private void Move(Vector2 direction)
  {
   Vector3 moveDirection = new Vector3(direction.x, 0, direction.y);
   WalkOrSprint(moveDirection);
  }

  private void WalkOrSprint(Vector3 moveDirection)
  {
   if (_inputService.IsSprintButtonPressed() && SprintRemaining > 0f && !_isSprintCooldown)
    Sprint(moveDirection);
   else
    Walk(moveDirection);

  }

  private void Walk(Vector3 moveDirection)
  {
   _isSprinting = false;
   moveDirection = transform.TransformDirection(moveDirection) * _speed;
   Vector3 velocityChange = VelocityChange(moveDirection);
   _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
  }

  private void Sprint(Vector3 moveDirection)
  {
   _isSprinting = true;
   moveDirection = transform.TransformDirection(moveDirection) * _sprintSpeed;
   Vector3 velocityChange = VelocityChange(moveDirection);
   _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
   
  }

  private void SprintStartedOrEnded()
  {
   if (_isSprinting)
    OnPlayerSprintStart?.Invoke();
   else
    OnPlayerSprintEnd?.Invoke();
  }

  private void SprintAvailableChecker()
  {

   if (_isSprinting)
   {
    SprintRemaining -= 1 * Time.deltaTime;
    if (SprintRemaining <= 0)
    {
     _isSprinting = false;
     _isSprintCooldown = true;
    }
   }
   
   else
    SprintRemaining = Mathf.Clamp(SprintRemaining += 1 * Time.deltaTime, 0, SprintDuration);

   if (_isSprintCooldown)
   {
    _sprintCooldownTimer -= 1 * Time.deltaTime;

    if (_sprintCooldownTimer <= 0)
    {
     _isSprintCooldown = false;
    }
   }

   else
    _sprintCooldownTimer = _sprintCooldownReset;
  }


  private Vector3 VelocityChange(Vector3 moveDirection)
  {
   Vector3 velocity = _rigidbody.velocity;
   Vector3 velocityChange = (moveDirection - velocity);
   velocityChange.x = Mathf.Clamp(velocityChange.x, -_maxVelocityChange, _maxVelocityChange);
   velocityChange.z = Mathf.Clamp(velocityChange.z, -_maxVelocityChange, _maxVelocityChange);
   velocityChange.y = 0;
   return velocityChange;
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
