using UnityEngine;

namespace CodeBase.CameraLogic
{
  public class CameraRotator : MonoBehaviour
  {
    [SerializeField] private Transform _camera;
    
    
    private void Update() => 
      _camera.Rotate(0, 10f * Time.deltaTime,0);
  }
}
