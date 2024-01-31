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
            CompareLeaderboardScores();
        }
        public void ShowLeaderboardUI()
        {
            if(PlayGamesPlatform.Instance.localUser.authenticated)
            {
                PlayGamesPlatform.Instance.ShowLeaderboardUI();
            }
        }
        private void CompareLeaderboardScores()
        {
            if(PlayGamesPlatform.Instance.localUser.authenticated)
            {
                ILeaderboard leaderboard = PlayGamesPlatform.Instance.CreateLeaderboard();
                leaderboard.timeScope = TimeScope.AllTime;

                leaderboard.id = GPGSIds.leaderboard_leaderboard;
                leaderboard.LoadScores(success => {
                    if(success)
                    {
                        long userLeaderboardScore = leaderboard.localUserScore.value;
                        var userLocalScore = VGPGSManager.Instance._playerData.highestScore;

                        if(userLocalScore > (int)userLeaderboardScore)// Player data's score is higher than the leaderboard so update leaderboard with data
                        {
                            DoLeaderboardPost(userLocalScore);
                        }
                        else// Player data's score is less than leaderboard score so update player data's highest score with leaderboard's
                        {
                            VGPGSManager.Instance._playerData.highestScore = (int)userLeaderboardScore;
                        }
                    }
                });
            }
        }
        private void DoLeaderboardPost(int _score)
        {
            if(PlayGamesPlatform.Instance.localUser.authenticated)
            {
                PlayGamesPlatform.Instance.ReportScore((long)_score, GPGSIds.leaderboard_leaderboard, (bool success) => {
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
        }
        public void PostScoreLeaderboard(int _scoreToPost)
        {
            DoLeaderboardPost(_scoreToPost);
        }
    }
}