using System;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.CameraLogic
{
  public class CameraRotator : MonoBehaviour
  {

    
    private void Update()
    {
      Vector3 rotationEnd = new Vector3(transform.rotation.x,90, transform.rotation.z);

      transform.DOLocalRotate(rotationEnd, 10, RotateMode.LocalAxisAdd)
        .From(transform.rotation.eulerAngles)
        .SetLoops(-1, LoopType.Yoyo);
    }
  }
}
