using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Runtime
{
    public class MenuUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tPlayerTag;
        [SerializeField] private TMP_Text _tStars;
        [SerializeField] private TMP_Text _tLevel;
        [SerializeField] private Image _levelSlider;

        private VPlayerData _playerData;
        private void Start() {
            _playerData = VGPGSManager.Instance._playerData;
            LoadMenuItems();
        }
        private void LoadMenuItems()
        {
            if(_tLevel.text != null)
                _tLevel.text = _playerData.level.ToString();
            if(_levelSlider != null)
            {
                var fillAmount = Mathf.Clamp01((float)_playerData.currentExperience / _playerData.neededExperience);
                _levelSlider.fillAmount = fillAmount;
            }
            if(Social.localUser.authenticated)
            {
                if(_tPlayerTag != null)
                    _tPlayerTag.text = Social.localUser.userName.ToString();
            }
            if(_tStars != null)
                _tStars.text = _playerData.stars.ToString();
        }
    }
}
