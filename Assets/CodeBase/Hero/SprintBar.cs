using System;
using CodeBase.Hero.PlayerController;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SprintBar : MonoBehaviour
    {
        [SerializeField] private Image _sprintBarBG;
        [SerializeField] private Image _sprintBar;
        [SerializeField] private PlayerMover _playerMover;
        [SerializeField] private CanvasGroup _sprintBarCG;
        private float _sprintBarWidth;
        private float _sprintBarHeight;
        private float _sprintBarHeightPercent = .015f;
        private float _sprintBarWidthPercent = .3f;
        private float _sprintRemainingPercent;


        private void Awake() =>
            SprintBarOn();


        private void SprintBarOn()
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            _sprintBarWidth = screenWidth * _sprintBarWidthPercent;
            _sprintBarHeight = screenHeight * _sprintBarHeightPercent;

            _sprintBarBG.rectTransform.sizeDelta = new Vector3(_sprintBarWidth, _sprintBarHeight, 0f);
            _sprintBar.rectTransform.sizeDelta = new Vector3(_sprintBarWidth - 2, _sprintBarHeight - 2, 0f);
        }


        public void TransformSprintBar()
        {
            _sprintRemainingPercent = PlayerMover.SprintRemaining / PlayerMover.SprintDuration;
            _sprintBar.transform.localScale = new Vector3(_sprintRemainingPercent, 1f, 1f);
        }

    }
}
