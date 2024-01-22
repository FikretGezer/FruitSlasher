using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

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
        public TMP_Text _loadInfoT;
        public VPlayerData _playerData;
        private const string fileName = "/player.dat";

        public static VGPGSManager Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
            Load();
        }
        #region Local Saving & Loading
        private void Save()
        {
            // Create a route from the program to the file
            var isExist = File.Exists(Application.persistentDataPath + fileName);
            FileStream file = isExist ? File.Open(Application.persistentDataPath + fileName, FileMode.Open) : File.Open(Application.persistentDataPath + fileName, FileMode.Create);

            // Create a copy of the save data
            VPlayerData pData = new VPlayerData();
            pData = _playerData;

            // Create a binary formatter that can read or write binary files
            BinaryFormatter formatter = new BinaryFormatter();
            // Save the data
            formatter.Serialize(file, pData);

            // Close the data stream
            file.Close();
        }
        private void Load()
        {
            if(File.Exists(Application.persistentDataPath + fileName))
            {
                // Open File
                FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);

                // Create a binary formatter that can read or write binary files
                BinaryFormatter formatter = new BinaryFormatter();
                _playerData = formatter.Deserialize(file) as VPlayerData;

                // Close the data stream
                file.Close();
                Debug.Log("Loaded");
                SceneManager.LoadScene("Menu");
            }
            else
            {
                Debug.Log("Loaded Sec");
                _playerData = new VPlayerData();
                SceneManager.LoadScene("Menu");
            }
        }
        #endregion


        #region SavedGames

        private bool isSaving;
        private void OpenSave(bool input) // If input true, activate SAVING otherwise activate LOADING
        {
            if(Social.localUser.authenticated)
            {
                isSaving = input;

                // Open to file to read data
                _loadInfoT.text = $"Signed in {Social.localUser.userName}\n";

                ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
                    "MyFileName",
                    DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime,
                    SaveOrLoadGameFile
                );
            }
            else
            {
                _loadInfoT.text = "Not signed in";
                // if(_playerData == null)
                // {
                //     _playerData = new VPlayerData();
                //     Debug.Log("Normal->Index: " + _playerData.unlockedBlades[_playerData.currentBladeIndex]);
                //     _loadInfoT.text += "NORMAL -> Index: " + _playerData.unlockedBlades[_playerData.currentBladeIndex] + "\n";
                // }
            }
        }
        private void SaveOrLoadGameFile(SavedGameRequestStatus status, ISavedGameMetadata meta)
        {
            if(status == SavedGameRequestStatus.Success)
            {
                _loadInfoT.text += "Saving or loading started.\n";
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
                    _loadInfoT.text += "Loading started...\n";

                    if(!meta.IsOpen)
                    {
                        _loadInfoT.text += "New User Created\n";
                        _playerData = new VPlayerData();
                        First.Instance.LoadNewScene("Menu");
                    }
                    else
                    {
                        _loadInfoT.text += "Existing Data is loading...\n";
                        ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(meta, CallbackLoad);
                    }
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
            var _pData = JsonUtility.FromJson<VPlayerData>(data);
            this._playerData = _pData;
            _loadInfoT.text += "CLOUD -> Player Data Loaded\nIs blade unlocked: " + _playerData.unlockedBlades[0] + "\n";
            First.Instance.LoadNewScene("Menu");
            // _playerData.neededExperience = (int)(_playerData.baseExperience * (_playerData.experienceMultiplier * _playerData.level));

            // if(output != null)
            // {
            //     LogOutput(output);
            // }
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
        // public TMP_Text output;
        // private void LogOutput(TMP_Text outputTxt)
        // {
        //     outputTxt.text = "High Score: " + _playerData.highestScore + ", Level: " + _playerData.level;
        // }
        #endregion
        private void OnDisable() {
            Save();
        }
        private void OnApplicationQuit() {
            Save();
            // OpenSave(true);
        }
    }


}