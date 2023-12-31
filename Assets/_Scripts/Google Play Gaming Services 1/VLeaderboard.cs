using UnityEngine;

namespace Runtime
{
    public class VLeaderboard : MonoBehaviour
    {
        public static VLeaderboard Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
        }
        public void ShowLeaderboardUI()
        {
            if(Social.localUser.authenticated)
            {
                Social.ShowLeaderboardUI();
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