using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using TMPro;

namespace Runtime
{
    public class VLeaderboard : MonoBehaviour
    {
        public TMP_Text logTxt;
        public TMP_InputField scoreInput;

        public void ShowLeaderboardUI()
        {
            Social.ShowLeaderboardUI();
        }
        private void DoLeaderboardPost(int _score)
        {
            Social.ReportScore(_score, GPGSIds.leaderboard_leaderboard, (bool success) => {
                if(success)
                {
                    logTxt.text = "Score posted of: " + _score;
                }
                else
                {
                    logTxt.text = "Score failed to post";
                }
            });
        }
        public void LeaderboardPostBtn()
        {
            DoLeaderboardPost(int.Parse(scoreInput.text));
        }
    }
}