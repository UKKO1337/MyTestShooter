using CodeBase.Services;
using CodeBase.Services.SaveLoad;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic
{
  public class SaveTrigger : MonoBehaviour, ISave
  {
    private ISaveLoadService _saveLoadService;

    public BoxCollider Collider;

    [Inject]
    private void Construct(ISaveLoadService saveLoadService) => 
      _saveLoadService = saveLoadService;
    

    private void OnTriggerEnter(Collider other)
    {
      SaveProgress();
      Debug.Log("Progress Saved.");
      gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
      if (!Collider)
        return;
      
      Gizmos.color = new Color(30,200,30,130);
      Gizmos.DrawCube(transform.position + Collider.center, Collider.size);
    }

    public void SaveProgress() => 
      _saveLoadService.SaveProgress();
  }
}