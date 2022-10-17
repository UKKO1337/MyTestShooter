using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
  [Serializable]
  public class KillData
  {
    public List<string> ClearedSpawners = new List<string>();

    public int Spawners;

    public Action Happend;

    public void Collect()
    {
      Spawners += 1;
      Happend?.Invoke();
    }
  }
}