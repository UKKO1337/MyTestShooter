using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Enemy
{
  public class KillCollector : MonoBehaviour
  {

    [SerializeField] private EnemyDeath _enemyDeath;
    private KillData _killData;

    public void Construct(KillData killData)
    {
      _killData = killData;
      _enemyDeath.Happend += Count;
    }

    private void Count()
    {
      _killData.Collect();
    }


  }
}