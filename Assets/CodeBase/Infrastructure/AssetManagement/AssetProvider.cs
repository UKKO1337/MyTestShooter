using UnityEngine;
namespace CodeBase.Infrastructure.AssetManagement
{
  public class AssetProvider : IAssets
    {
      public GameObject Instantiate(string path, Vector3 at)
      {
        var prefab = UnityEngine.Resources.Load<GameObject>(path);
        return Object.Instantiate(prefab, at, Quaternion.identity);
      }

      public GameObject Instantiate(string path)
      {
        var prefab = UnityEngine.Resources.Load<GameObject>(path);
        return Object.Instantiate(prefab);
      }
    }
}

