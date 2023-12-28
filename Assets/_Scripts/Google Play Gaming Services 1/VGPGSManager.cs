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
        public long highestScore = 0;
        public int level = 1;
        public int experience = 0;
        public bool[] achievements;
        public bool[] blades;
        public bool[] backgrounds;
    }
    public class VGPGSManager : MonoBehaviour
    {
        public VPlayerData _playerData { get; set; }

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
        #endregion
    }
}