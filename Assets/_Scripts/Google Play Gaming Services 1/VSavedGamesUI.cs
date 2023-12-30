using GooglePlayGames;
using UnityEngine;
using TMPro;

namespace Runtime
{
    public class VSavedGamesUI : MonoBehaviour
    {
        // public TMP_Text outputTxt;
        // public void IncreaseScore()
        // {
        //     var score = VGPGSManager.Instance._playerData.highestScore;
        //     score += 10;
        //     VGPGSManager.Instance._playerData.highestScore = score;
        //     VGPGSManager.Instance.OpenSave(true);
        // }
        // public void LoadScore()
        // {
        //     if(outputTxt != null)
        //     {
        //         VGPGSManager.Instance.OpenSave(false);
        //         VGPGSManager.Instance.output = outputTxt;
        //     }
        // }
        // public void IncreaseLevel() {
        //     VGPGSManager.Instance._playerData.level++;
        //     VGPGSManager.Instance.OpenSave(true);
        // }
        [SerializeField] private TMP_Text _tHighestScore;
        [SerializeField] private TMP_Text _tLevel;
        public static VSavedGamesUI Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
        }
        #region In Game Updates
        public void UpdateHighestScore(int currentScore)
        {
            var playerData = VGPGSManager.Instance._playerData;

            if(currentScore > playerData.highestScore)
            {
                playerData.highestScore = currentScore;
                _tHighestScore.text = currentScore.ToString();

                VGPGSManager.Instance.OpenSave(true);
                PostScoreToLeaderboard(currentScore);
            }
            else
            {
                _tHighestScore.text = playerData.highestScore.ToString();
                PostScoreToLeaderboard(currentScore);
            }
        }
        public void CalculateExperience(int fruitCutAmount)
        {
            int experience = fruitCutAmount;

            IncreaseXP(experience);
        }
        private void IncreaseXP(int experience)
        {
            var _playerData = VGPGSManager.Instance._playerData;
            _playerData.currentExperience += experience;

            if(_playerData.currentExperience >= _playerData.neededExperience)
            {
                _playerData.currentExperience = _playerData.currentExperience % _playerData.neededExperience;

                _playerData.level++;
                _tLevel.text = _playerData.level.ToString();
                _playerData.neededExperience = (int)(_playerData.baseExperience * (_playerData.experienceMultiplier * _playerData.level));
            }

            VGPGSManager.Instance.OpenSave(true);
        }
        private void PostScoreToLeaderboard(int scoreToPost)
        {
            if(FindObjectOfType<VLeaderboard>() != null)
                VLeaderboard.Instance.PostScoreLeaderboard(scoreToPost);
        }
        #endregion
    }
}
