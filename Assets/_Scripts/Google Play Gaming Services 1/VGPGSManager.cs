using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;
using TMPro;
using System;

namespace Runtime
{
    [Serializable]
    public class VPlayerData {
        public int highestScore;
        public int level;
        public int stars;

        public float experienceMultiplier;
        public int baseExperience;
        public int neededExperience;
        public int currentExperience;

        public int currentBladeIndex;
        public int currentDojoIndex;

        public bool[] UnlockedAchievements;
        public bool[] unlockedBlades;
        public bool[] boughtBlades;
        public bool[] unlockedDojos;
        public bool[] boughtDojos;

        public bool _areMenuTipsDone;
        public bool _arePreGameTipsDone;

        public bool areNewBladesUnlocked;
        public bool areNewDojosUnlocked;

        #region Achievements
        public bool achivement_sliceFirstFruit;
        public bool achivement_fruitSalad;
        public bool achivement_tastyQuadro;
        public bool achivement_comboBeginner;
        public bool achivement_berryFan;
        public bool achivement_pulpFiction;
        public bool achivement_sliceMaster;
        public bool achivement_fruitNinjaApprentice;
        public bool achivement_orangeBlitz;
        public bool achivement_comboProdigy;
        public bool achivement_fruitNinjaMaster;
        public bool achivement_comboVirtuoso;
        public bool achivement_fruitExtravaganza;
        public bool achivement_fruitNinjaLegend;
        #endregion

        public VPlayerData() {
            highestScore = 0;
            level = 1;
            stars = 500;

            experienceMultiplier = 1.2f;
            baseExperience = 80;
            neededExperience = (int)(baseExperience * (experienceMultiplier * level));
            currentExperience = 0;

            currentBladeIndex = 0;
            currentDojoIndex = 0;

            UnlockedAchievements = new bool[20];
            unlockedBlades = new bool[20];
            unlockedDojos = new bool[20];
            boughtBlades = new bool[20];
            boughtDojos = new bool[20];

            unlockedBlades[0] = true;
            boughtBlades[0] = true;
            unlockedDojos[0] = true;
            boughtDojos[0] = true;

            _areMenuTipsDone = false;
            _arePreGameTipsDone = false;

            areNewBladesUnlocked = false;
            areNewDojosUnlocked = false;
        }


    }
    public class VGPGSManager : MonoBehaviour
    {
        public VPlayerData _playerData {get;set;}

        public static VGPGSManager Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
            OpenSave(false);
        }

        #region SavedGames

        private bool isSaving;
        public void OpenSave(bool input) // If input true, activate SAVING otherwise activate LOADING
        {
            if(Social.localUser.authenticated)
            {
                isSaving = input;

                // Open to file to read data

                ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
                    "MyFileName",
                    DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime,
                    SaveOrLoadGameFile
                );
            }
            // else
            // {
            //     if(_playerData == null)
            //     {
            //         _playerData = new VPlayerData();
            //         Debug.Log("Index: " + _playerData.unlockedBlades[_playerData.currentBladeIndex]);
            //     }
            // }
        }
        private void SaveOrLoadGameFile(SavedGameRequestStatus status, ISavedGameMetadata meta)
        {
            if(status == SavedGameRequestStatus.Success)
            {
                if(isSaving) // Saving
                {

                    byte[] byteData = System.Text.ASCIIEncoding.ASCII.GetBytes(GetSaveString());

                    SavedGameMetadataUpdate updateForMetadata = new SavedGameMetadataUpdate.Builder().WithUpdatedDescription("Player Data Updated at " + DateTime.Now.ToString()).Build();

                    ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(
                        meta,
                        updateForMetadata,
                        byteData,
                        CallbackSave
                    );
                }
                else // Loading
                {
                    ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(meta, CallbackLoad);
                }
            }
            else
            {

            }
        }
        private string GetSaveString()
        {
            string stringData = JsonUtility.ToJson(_playerData);

            return stringData;
        }
        private void CallbackLoad(SavedGameRequestStatus status, byte[] data)
        {
            if(status == SavedGameRequestStatus.Success)
            {
                string playerDataString = System.Text.ASCIIEncoding.ASCII.GetString(data);

                LoadSavedData(playerDataString);
            }
            else
            {

            }
        }
        private void LoadSavedData(string data)
        {
            var _playerData = JsonUtility.FromJson<VPlayerData>(data);
            this._playerData = _playerData;

            // _playerData.neededExperience = (int)(_playerData.baseExperience * (_playerData.experienceMultiplier * _playerData.level));

            if(output != null)
            {
                LogOutput(output);
            }
        }
        private void CallbackSave(SavedGameRequestStatus status, ISavedGameMetadata meta)
        {
            if(status == SavedGameRequestStatus.Success)
            {

            }
            else
            {

            }
        }
        public TMP_Text output;
        private void LogOutput(TMP_Text outputTxt)
        {
            outputTxt.text = "High Score: " + _playerData.highestScore + ", Level: " + _playerData.level;
        }
        private void OnDisable() {
            OpenSave(true);
        }
        private void OnApplicationQuit() {
            OpenSave(true);
        }
        #endregion
    }
}