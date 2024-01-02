using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Runtime
{
    public class VLeaderboard : MonoBehaviour
    {
        public static VLeaderboard Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
            CompareLeaderboardScores();
        }
        public void ShowLeaderboardUI()
        {
            if(Social.localUser.authenticated)
            {
                Social.ShowLeaderboardUI();
            }
        }
        private void CompareLeaderboardScores()
        {
            if(Social.localUser.authenticated)
            {
                ILeaderboard leaderboard = Social.CreateLeaderboard();
                leaderboard.timeScope = TimeScope.AllTime;

                leaderboard.id = GPGSIds.leaderboard_leaderboard;
                leaderboard.LoadScores(success => {
                    if(success)
                    {
                        long userLeaderboardScore = leaderboard.localUserScore.value;
                        var userLocalScore = VGPGSManager.Instance._playerData.highestScore;

                        if(userLocalScore > (int)userLeaderboardScore)
                        {
                            DoLeaderboardPost(userLocalScore);
                        }
                    }
                });
            }
        }
        private void DoLeaderboardPost(int _score)
        {
            if(Social.localUser.authenticated)
            {
                Social.ReportScore(_score, GPGSIds.leaderboard_leaderboard, (bool success) => {
                    if(success)
                    {
                        Debug.Log("Score posted of: " + _score);
                    }
                    else
                    {
                        Debug.Log("Score failed to post");
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