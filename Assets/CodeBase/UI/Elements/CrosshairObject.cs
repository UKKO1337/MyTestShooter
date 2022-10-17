using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
  [RequireComponent(typeof(Image))]
  public class CrosshairObject : MonoBehaviour
  {
    private Image _crosshairObject;
    [Header("Crosshair color")] [SerializeField] private Color _crosshairColor = Color.white;
    [Header("Crosshair image")] [SerializeField] private Sprite _crosshairImage;

    private void Start()
    {
      _crosshairObject = GetComponent<Image>();
      _crosshairObject.sprite = _crosshairImage;
      _crosshairObject.color = _crosshairColor;
    }

    public void TurnOnCrosshair()
    {
      _crosshairObject.gameObject.SetActive(true);
    }

    public void TurnOffCrosshair()
    {
      _crosshairObject.gameObject.SetActive(false);
    }
  }
}
