using UnityEngine;

namespace CodeBase.StaticData
{
  [CreateAssetMenu(fileName = "PlayerData", menuName = "StaticData/Player")]
  public class PlayerStaticData : ScriptableObject
  {
    [Range(0, 2)] 
    public int FireRate;

    [Range(10, 100)] 
    public int Damage;

    [Range(15, 30)] 
    public int RangeOfAttack;

    [Range(10f, 80f)] 
    public float MouseSense;

    [Range(10, 100)] 
    public int Hp;
  }
}