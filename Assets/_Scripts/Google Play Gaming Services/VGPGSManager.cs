using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;
using UnityEngine.Android;
using TMPro;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine.SceneManagement;

namespace Runtime
{
    public class VGPGSManager : MonoBehaviour
    {
        public TMP_Text _infoCloudT;
        public TMP_Text _infoT;
        public TMP_Text _loadInfoT;
        public VPlayerData _playerData;
        private bool isSaving;
        private string fileName = "player.game";
        private const string cloudFileName = "CurrentPlayerData";
        private readonly string encryptionCodeWord = "word";

        public static VGPGSManager Instance;
        private void Awake() {
            if(Instance == null) Instance = this;

            _playerData = new VPlayerData();
            _infoCloudT.text = "CLOUD\n";
            _infoT.text = "NORMAL\n";
            // Load();
        }
        #region Local Saving & Loading
        public void Load()
        {
            string fullPath = Path.Combine(Application.persistentDataPath, fileName);
            _playerData = null;
            if(File.Exists(fullPath))
            {
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                dataToLoad = EncrypDecrypt(dataToLoad);

                _playerData = JsonUtility.FromJson<VPlayerData>(dataToLoad);

                AsyncLoader.Instance.LoadSceneAsync("Menu");
            }
            else
            {
                _playerData = new VPlayerData();
                AsyncLoader.Instance.LoadSceneAsync("Menu");
            }
        }
        public void Save()
        {
            string fullPath = Path.Combine(Application.persistentDataPath, fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(_playerData, true);

            dataToStore = EncrypDecrypt(dataToStore);

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        private string EncrypDecrypt(string data)
        {
            string modifiedData = "";

            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
            }

            return modifiedData;
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
                VLeaderboard.Instance.CompareLeaderboardScores();
                // if(_playerData != null) SceneManager.LoadScene("Menu");
                if(_playerData != null)
                {
                    AsyncLoader.Instance.LoadSceneAsync("Menu");
                }
                else
                {
                    _playerData = new VPlayerData();

                    OpenSavedGame(true);
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