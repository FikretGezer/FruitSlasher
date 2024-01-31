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
using System.Text;

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

        public float musicVolume;
        public float soundFXVolume;
        public float totalPlayTime;

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

            musicVolume = 0f;
            soundFXVolume = 0f;

            totalPlayTime = 0f;
        }
    }
    public class VGPGSManager : MonoBehaviour
    {
        public TMP_Text _infoCloudT;
        public TMP_Text _infoT;
        public TMP_Text _loadInfoT;
        public VPlayerData _playerData;
        private bool isSaving;
        private const string fileName = "/player.dat";
        private const string cloudFileName = "CurrentPlayerData";

        public static VGPGSManager Instance;
        private void Awake() {
            if(Instance == null) Instance = this;

            _playerData = new VPlayerData();
            _infoCloudT.text = "CLOUD\n";
            _infoT.text = "NORMAL\n";
            // Load();
        }
        #region Local Saving & Loading
        public void Save()
        {
            // Create a route from the program to the file
            string filePath = Path.Combine(Application.persistentDataPath, fileName);

            var isExist = File.Exists(filePath);
            FileStream file = isExist ? File.Open(filePath, FileMode.Open) : File.Open(filePath, FileMode.Create);

            // Create a copy of the save data
            VPlayerData pData = new VPlayerData();
            pData = _playerData;

            // Create a binary formatter that can read or write binary files
            BinaryFormatter formatter = new BinaryFormatter();
            // Save the data
            formatter.Serialize(file, pData);

            // Close the data stream
            file.Close();
            _infoT.text += $"File Saved Normal\n";
        }
        public void Load()
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            if(File.Exists(filePath))
            {
                // Open File
                FileStream file = File.Open(filePath, FileMode.Open);

                // Create a binary formatter that can read or write binary files
                BinaryFormatter formatter = new BinaryFormatter();
                _playerData = formatter.Deserialize(file) as VPlayerData;

                // Close the data stream
                file.Close();
                Debug.Log("Loaded");
                _infoT.text += "File Exist, Loaded Normal\n";
                // SceneManager.LoadScene("Menu");
                AsyncLoader.Instance.LoadSceneAsync("Menu");
            }
            else
            {
                Debug.Log("Loaded Sec");
                _playerData = new VPlayerData();
                _infoT.text += "File Not Found Created and Loaded Normal\n";
                // SceneManager.LoadScene("Menu");
                AsyncLoader.Instance.LoadSceneAsync("Menu");
            }
        }

        public void DeleteLocalGameData()
        {
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        #endregion

        #region Cloud Save & Load
        public void OpenSavedGame(bool input)
        {
            if(PlayGamesPlatform.Instance.IsAuthenticated())
            {
                isSaving = input;
                _infoCloudT.text = "File Opened Cloud\n";
                ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
                savedGameClient.OpenWithAutomaticConflictResolution(
                    cloudFileName,
                    DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime,
                    OnSavedGameOpened
                );
            }
        }
        private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata meta)
        {
            if(status == SavedGameRequestStatus.Success)
            {
                _infoCloudT.text += "File Success Cloud\n";
                if(isSaving)
                {
                    _infoCloudT.text += "File Saving Cloud\n";
                    SaveGame(meta);
                }
                else
                {
                    _infoCloudT.text += "File Loading Cloud\n";
                    LoadGameData(meta);
                }
            }
            else
            {

            }
        }
        private void SaveGame(ISavedGameMetadata game)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

            builder = builder
                .WithUpdatedPlayedTime(game.TotalTimePlayed)
                .WithUpdatedDescription("Saved game at " + DateTime.Now);

            string playerDataString = JsonUtility.ToJson(_playerData);
            byte[] data = ASCIIEncoding.ASCII.GetBytes(playerDataString);

            SavedGameMetadataUpdate updatedMetaData = builder.Build();
            savedGameClient.CommitUpdate(game, updatedMetaData, data, OnSavedGameWritten);
        }
        private void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            if(status == SavedGameRequestStatus.Success)
            {
                _infoCloudT.text += "Final File Saving Success Cloud\n";
            }
            else
            {

            }
        }
        private void LoadGameData(ISavedGameMetadata game)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
        }
        private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
        {
            if(status == SavedGameRequestStatus.Success)
            {
                string playerDataString = ASCIIEncoding.ASCII.GetString(data);
                _playerData = JsonUtility.FromJson<VPlayerData>(playerDataString);
                _infoCloudT.text += "Final File Loading Success Cloud\n";
                // if(_playerData != null) SceneManager.LoadScene("Menu");
                if(_playerData != null) AsyncLoader.Instance.LoadSceneAsync("Menu");
                else
                {
                    _playerData = new VPlayerData();
                    OpenSavedGame(true);
                    // SceneManager.LoadScene("Menu");
                    AsyncLoader.Instance.LoadSceneAsync("Menu");
                }
            }
            else
            {

            }
        }

        public void DeleteGameData()
        {
            if(PlayGamesPlatform.Instance.IsAuthenticated())
            {
                ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
                savedGameClient.OpenWithAutomaticConflictResolution(
                    cloudFileName,
                    DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime,
                    DeleteSavedGame
                );
            }
        }
        private void DeleteSavedGame(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            if(status == SavedGameRequestStatus.Success)
            {
                ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
                savedGameClient.Delete(game);
            }
        }
        #endregion

        public void SaveDouble()
        {
            OpenSavedGame(true);
            Save();
        }
        private void OnEnable() {
            EventManager.AddHandler(GameEvents.OnEndGameUIUpdateFinish, SaveDouble);
        }
        private void OnDisable() {
            EventManager.RemoveHandler(GameEvents.OnEndGameUIUpdateFinish, SaveDouble);
        }
    }


}