using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
namespace Runtime
{
    public class VAchievement : MonoBehaviour
    {
        public static VAchievement Instance;
        private void Awake() {
            if(Instance == null) Instance = this;

            extravaganzaTag.Clear();
            saladTags.Clear();

            tagCompleteCount = 0;
        }
        private void Update() {
            // DequeueAchievements();
        }
        internal void ShowAchievementsUI()
        {
            PlayGamesPlatform.Instance.ShowAchievementsUI();
        }
        internal void DoGrantAchievement(string _achievement)
        {
            if(PlayGamesPlatform.Instance.IsAuthenticated())
            {
                PlayGamesPlatform.Instance.ReportProgress(_achievement, 100.0f, (bool success) => {
                    if(success)
                    {
                        // LoadAchievementInfo(_achievement);
                        //perform new actions on success
                    }
                    else
                    {

                    }
                });
            }
        }
        internal void DoIncrementalAchievement(string _achievement)
        {
            if(PlayGamesPlatform.Instance.IsAuthenticated())
            {
                PlayGamesPlatform.Instance.IncrementAchievement(_achievement, 1, (bool success) => {
                    if(success)
                    {

                        //perform new actions on success
                        // LoadAchievementInfo(_achievement);
                    }
                    else
                    {

                    }
                });
            }
        }
        internal void DoRevealAchievement(string _achievement)
        {
            PlayGamesPlatform.Instance.ReportProgress(_achievement, 0f, (bool success) => {
                if(success)
                {

                    //perform new actions on success
                }
                else
                {

                }
            });
        }

        internal void ListAchievements()
        {
            PlayGamesPlatform.Instance.LoadAchievements(achievements => {

                foreach (IAchievement ach in achievements)
                {

                }
            });
        }
        internal void ListDescriptions()
        {
            PlayGamesPlatform.Instance.LoadAchievementDescriptions(achievements => {

                foreach (IAchievementDescription ach in achievements)
                {

                }
            });
        }
        public void ShowAchievements()
        {
            ShowAchievementsUI();
        }

        public void AchievementJuicyStart() => DoGrantAchievement(GPGSIds.achievement_juicy_start);
        public void AchievementFruitNovice() => DoIncrementalAchievement(GPGSIds.achievement_fruit_novice);
        private void AchievementFruitSalad() => DoGrantAchievement(GPGSIds.achievement_fruit_salad);
        public void AchievementTastyQuadro() => DoGrantAchievement(GPGSIds.achievement_tasty_quadro);
        public void AchievementComboBeginner() => DoGrantAchievement(GPGSIds.achievement_combo_beginner);
        public void AchievementBerryFan() => DoIncrementalAchievement(GPGSIds.achievement_berry_fan);
        public void AchievementPulpFiction() => DoGrantAchievement(GPGSIds.achievement_pulp_fiction);
        public void AchievementSliceMaster() => DoGrantAchievement(GPGSIds.achievement_slice_master);
        public void AchievementFruitNinjaApprentice() => DoGrantAchievement(GPGSIds.achievement_fruit_ninja_apprentice);
        public void AchievementOrangeBlitz() => DoIncrementalAchievement(GPGSIds.achievement_orange_blitz);
        public void AchievementComboProdidy() => DoIncrementalAchievement(GPGSIds.achievement_combo_prodigy);
        public void AchievementFruitNinjaMaster() => DoGrantAchievement(GPGSIds.achievement_fruit_ninja_master);
        public void AchievementComboVirtuoso() => DoIncrementalAchievement(GPGSIds.achievement_combo_virtuoso);
        private void AchievementFruitExtravaganza() => DoGrantAchievement(GPGSIds.achievement_fruit_extravaganza);
        public void AchievementFruitNinjaLegend() => DoGrantAchievement(GPGSIds.achievement_fruit_ninja_legend);

        #region Fruit Salad
        private List<string> saladTags = new List<string>();
        private readonly string[] fruitTags = {"greenApple","lemon","lime","orange","peach","pear","redApple","starFruit","strawberry","watermelon"};
        public void UnlockFruitSalad(string tag)
        {
            if(!saladTags.Contains(tag) && saladTags.Count < fruitTags.Length)
            {
                saladTags.Add(tag);
            }
            if(saladTags.Count >= fruitTags.Length)
            {
                AchievementFruitSalad();
            }
        }
        #endregion
        #region Fruit Extravaganza
        private List<string> extravaganzaTag = new List<string>();
        private int tagCompleteCount = 0;
        public void UnlockFruitExtravaganza(string tag)
        {
            if(!extravaganzaTag.Contains(tag) && extravaganzaTag.Count < fruitTags.Length)
            {
                extravaganzaTag.Add(tag);
            }
            if(extravaganzaTag.Count >= fruitTags.Length)
            {
                extravaganzaTag.Clear();
                tagCompleteCount++;
            }
            if(tagCompleteCount >= 5)
                AchievementFruitExtravaganza();
        }
        #endregion
    }
}