using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using TMPro;

namespace Runtime
{
    public class VAchievement : MonoBehaviour
    {
        public TMP_Text logTxt;
        internal void ShowAchievementsUI()
        {
            Social.ShowAchievementsUI();
        }
        internal void DoGrantAchievement(string _achievement)
        {
            Social.ReportProgress(_achievement, 100.0f, (bool success) => {
                if(success)
                {
                    logTxt.text = _achievement + " : " + success.ToString();
                    //perform new actions on success
                }
                else
                {
                    logTxt.text = _achievement + " : " + success.ToString();
                }
            });
        }
        internal void DoIncrementalAchievement(string _achievement)
        {
            PlayGamesPlatform platform = (PlayGamesPlatform)Social.Active;

            platform.IncrementAchievement(_achievement, 1, (bool success) => {
                if(success)
                {
                    logTxt.text = _achievement + " : " + success.ToString();
                    //perform new actions on success
                }
                else
                {
                    logTxt.text = _achievement + " : " + success.ToString();
                }
            });
        }
        internal void DoRevealAchievement(string _achievement)
        {
            Social.ReportProgress(_achievement, 0f, (bool success) => {
                if(success)
                {
                    logTxt.text = _achievement + " : " + success.ToString();
                    //perform new actions on success
                }
                else
                {
                    logTxt.text = _achievement + " : " + success.ToString();
                }
            });
        }
        internal void ListAchievements()
        {
            Social.LoadAchievements(achievements => {
                logTxt.text = "Loaded Achievements " + achievements.Length;
                foreach (IAchievement ach in achievements)
                {
                    logTxt.text += "/n" + ach.id + " " + ach.completed;
                }
            });
        }
        internal void ListDescriptions()
        {
            Social.LoadAchievementDescriptions(achievements => {
                logTxt.text = "Loaded Achievements " + achievements.Length;
                foreach (IAchievementDescription ach in achievements)
                {
                    logTxt.text += "/n" + ach.id + " " + ach.title;
                }
            });
        }
        public void ShowAchievements()
        {
            ShowAchievementsUI();
        }
        public void ShowListAchievements()
        {
            ListAchievements();
        }
        public void ShowListDescription()
        {
            ListDescriptions();
        }
        public void GrantAchievementButton()
        {
            DoGrantAchievement(GPGSIds.achievement_unlock_achievement);
        }
        public void GrantIncrementalButton()
        {
            DoIncrementalAchievement(GPGSIds.achievement_incremental_achievement);
        }
        public void RevealAchievementButton()
        {
            DoIncrementalAchievement(GPGSIds.achievement_hidden_unlock_achievement);
        }
        public void RevealIncrementalButton()
        {
            DoIncrementalAchievement(GPGSIds.achievement_hidden_incremental_achievement);
        }
        public void GrantHiddenAchievementButton()
        {
            DoGrantAchievement(GPGSIds.achievement_hidden_unlock_achievement);
        }
        public void HiddenIncrementalAchievementButton()
        {
            DoIncrementalAchievement(GPGSIds.achievement_hidden_incremental_achievement);
        }
    }
}