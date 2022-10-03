using CodeBase.Logic;
using UnityEngine;

namespace Resources.StaticData
{
  [CreateAssetMenu(fileName = "ZombieData", menuName = "StaticData/Zombie")]
  public class ZombieStaticData : ScriptableObject
  {
    public ZombieTypeId ZombieTypeId;
    
    [Range(1, 100)]
    public int Hp;
    
    [Range(1f, 30f)]
    public float Damage;
    
    [Range(0.5f, 1f)]
    public float EffectiveDistance = 0.666f;
    
    [Range(0.5f, 1f)]
    public float Cleavage;

    [Range(1f, 5f)] 
    public float MoveSpeed;

    public GameObject Prefab;
  }
}