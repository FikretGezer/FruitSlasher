using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using GooglePlayGames;

namespace Runtime
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tPlayerTag;
        [SerializeField] private TMP_Text _tStars;
        [SerializeField] private TMP_Text _tLevel;
        [SerializeField] private Image _levelSlider;

        public static MenuUI Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
        }
        private void Start() {
            LoadMenuItems();
        }
        private void LoadMenuItems()
        {
            if(_tLevel != null)
                _tLevel.text = VGPGSManager.Instance._playerData.level.ToString();
            if(_levelSlider != null)
            {
                var fillAmount = Mathf.Clamp01((float)VGPGSManager.Instance._playerData.currentExperience / VGPGSManager.Instance._playerData.neededExperience);
                _levelSlider.fillAmount = fillAmount;
            }
            if(_tPlayerTag != null)
            {
                if(PlayGamesPlatform.Instance.IsAuthenticated())
                    _tPlayerTag.text = PlayGamesPlatform.Instance.localUser.userName.ToString();
            }
            SetStars();
        }
        public void SetStars()
        {
            if(_tStars != null)
                _tStars.text = VGPGSManager.Instance._playerData.stars.ToString();
        }
    }
}
