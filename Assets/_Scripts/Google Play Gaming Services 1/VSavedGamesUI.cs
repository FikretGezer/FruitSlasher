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
        public static VSavedGamesUI Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
        }
        public void UpdateHighestScore(long currentScore)
        {
            if(Social.localUser.authenticated)
            {
                var playerData = VGPGSManager.Instance._playerData;

                if(currentScore > playerData.highestScore)
                {
                    playerData.highestScore = currentScore;
                    _tHighestScore.text = currentScore.ToString();
                    VGPGSManager.Instance.OpenSave(true);
                }
                else
                {
                    _tHighestScore.text = playerData.highestScore.ToString();
                }
            }
        }
    }
}
