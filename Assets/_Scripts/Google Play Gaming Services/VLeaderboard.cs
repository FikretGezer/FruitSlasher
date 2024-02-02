using System;
using GooglePlayGames;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Runtime
{
    public class VLeaderboard : MonoBehaviour
    {
        public static VLeaderboard Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
        }
        private void Start()
        {
            // CompareLeaderboardScores();
        }
        public void ShowLeaderboardUI()
        {
            if(PlayGamesPlatform.Instance.IsAuthenticated())
            {
                PlayGamesPlatform.Instance.ShowLeaderboardUI();
            }
        }
        public void CompareLeaderboardScores()
        {
            if(PlayGamesPlatform.Instance.IsAuthenticated())
            {
                ILeaderboard leaderboard = PlayGamesPlatform.Instance.CreateLeaderboard();
                // leaderboard.timeScope = TimeScope.AllTime;

                leaderboard.id = GPGSIds.leaderboard_leaderboard;
                leaderboard.LoadScores(success => {
                    if(success)
                    {
                        var userLeaderboardScore = leaderboard.localUserScore.value;
                        var userLocalScore = VGPGSManager.Instance._playerData.highestScore;

                        if(userLocalScore > userLeaderboardScore)// Player data's score is higher than the leaderboard so update leaderboard with data
                        {
                            PostScoreLeaderboard(userLocalScore);
                        }
                        else// Player data's score is less than leaderboard score so update player data's highest score with leaderboard's
                        {
                            VGPGSManager.Instance._playerData.highestScore = Convert.ToInt32(userLeaderboardScore);
                        }
                    }
                });
            }
        }
        private void DoLeaderboardPost(int _score)
        {
            PlayGamesPlatform.Instance.ReportScore(_score, GPGSIds.leaderboard_leaderboard, (bool success) => {
                if(success)
                {
                    // Debug.Log("Score posted of: " + _score);
                }
                else
                {
                    // Debug.Log("Score failed to post");
                }
            });
        }
        public void PostScoreLeaderboard(int _scoreToPost)
        {
            if(PlayGamesPlatform.Instance.IsAuthenticated())
            {
                DoLeaderboardPost(_scoreToPost);
            }
        }
    }
}