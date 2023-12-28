using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;
using TMPro;
using System;

namespace Runtime
{
    [Serializable]
    public class PlayerData {
        public long highestScore = 0;
        public int level = 1;
        public int experience = 0;
        public bool[] achievements;
        public bool[] blades;
        public bool[] backgrounds;
    }
    public class GPGSManager : MonoBehaviour
    {
        public TMP_Text statusTxt;
        public TMP_Text descriptionTxt;
        public GameObject achievementButton;
        public GameObject leaderboardButton;
        public GameObject savePanelButton;
        public GameObject authenticateButton;
        public SavedGamesUI _savedGamesUI;

        public static GPGSManager Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
        }
        private void Start() {
            SignInToGPGS();
        }
        private void OnEnable() {
            OpenSave(false);
        }
        #region Authentication
        private void SignInToGPGS()
        {
            PlayGamesPlatform.Activate();

            if(!Social.localUser.authenticated)
            {
                PlayGamesPlatform.Instance.Authenticate(SignInCallback);
            }
        }
        private void SignInCallback(SignInStatus status)
        {
            if(status == SignInStatus.Success)
            {
                statusTxt.text = "Signed in successfully";
                descriptionTxt.text = "Account Name: " + Social.localUser.userName;
                achievementButton.SetActive(true);
                leaderboardButton.SetActive(true);
                savePanelButton.SetActive(true);
            }
            else
            {
                statusTxt.text = "Signing in failed...";

                // Activate manually signing in
                authenticateButton.SetActive(true);
            }
        }
        public void AuthenticateButton()
        {
            PlayGamesPlatform.Instance.ManuallyAuthenticate(ManuallyAuthenticateCallback);
        }
        private void ManuallyAuthenticateCallback(SignInStatus status)
        {
            if(status == SignInStatus.Success)
            {
                statusTxt.text = "Signed in successfully";
                descriptionTxt.text = "Account Name: " + Social.localUser.userName;
                achievementButton.SetActive(true);
                leaderboardButton.SetActive(true);
                savePanelButton.SetActive(true);
                authenticateButton.SetActive(false);
            }
            else
            {
                statusTxt.text = "Signing in failed...";
            }
        }
        #endregion

        #region SavedGames

        private bool isSaving;
        public void OpenSave(bool input) // If input true, activate SAVING otherwise activate LOADING
        {
            if(Social.localUser.authenticated)
            {
                isSaving = input;

                _savedGamesUI.logTxt.text = "File Opened For Saving/Loading. ";

                // Open to file to read data
                _savedGamesUI.logTxt.text += Social.localUser.userName + " is authenticated. ";
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
                    _savedGamesUI.logTxt.text += "Saving started... ";
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
                    _savedGamesUI.logTxt.text += "Loading started... ";
                    ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(meta, CallbackLoad);
                }
            }
            else
            {
                _savedGamesUI.logTxt.text += "Unable to load/save... ";
            }
        }
        private void CallbackLoad(SavedGameRequestStatus status, byte[] data)
        {
            if(status == SavedGameRequestStatus.Success)
            {
                _savedGamesUI.logTxt.text += "Load successful, trying read the loaded data. ";
                string playerDataString = System.Text.ASCIIEncoding.ASCII.GetString(data);

                LoadSavedData(playerDataString);
            }
            else
            {
                _savedGamesUI.logTxt.text += "Unable to load the data. ";
            }
        }
        private void LoadSavedData(string data)
        {
            _savedGamesUI.logTxt.text += "Player Data is readed successfully. ";

            var _playerData = JsonUtility.FromJson<PlayerData>(data);
            _savedGamesUI._playerData = _playerData;
            _savedGamesUI.outputTxt.text = "High Score: " + _playerData.highestScore.ToString() + "Level: " + _playerData.level.ToString();

            // string[] playerData = data.Split('-');
            // _savedGamesUI.name = playerData[0];
            // _savedGamesUI.age = int.Parse(playerData[0]);

            // _savedGamesUI.outputTxt.text = "Name: "+ _savedGamesUI.name + ", Age: " + _savedGamesUI.age;
        }
        private void CallbackSave(SavedGameRequestStatus status, ISavedGameMetadata meta)
        {
            if(status == SavedGameRequestStatus.Success)
            {
                _savedGamesUI.logTxt.text += "Player data saved successfully. ";
            }
            else
            {
                _savedGamesUI.logTxt.text += "Player data saving failed. ";
            }
        }
        private string GetSaveString()
        {
            string stringData = JsonUtility.ToJson(_savedGamesUI._playerData);
            // string stringData = _savedGamesUI.name + "-" + _savedGamesUI.age;

            return stringData;
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
// using GooglePlayGames;
// using GooglePlayGames.BasicApi;
// using GooglePlayGames.BasicApi.SavedGame;
// using UnityEngine;
// using TMPro;
// using System;

// namespace Runtime
// {
//     [Serializable]
//     public class PlayerData {
//         public long highestScore = 0;
//         public int level = 1;
//         public int experience = 0;
//         public bool[] achievements;
//         public bool[] blades;
//         public bool[] backgrounds;
//     }
//     public class GPGSManager : MonoBehaviour
//     {
//         public TMP_Text statusTxt;
//         public TMP_Text descriptionTxt;
//         public GameObject achievementButton;
//         public GameObject leaderboardButton;
//         public GameObject savePanelButton;
//         public GameObject authenticateButton;
//         public SavedGamesUI _savedGamesUI;

//         public static GPGSManager Instance;
//         private void Awake() {
//             if(Instance == null) Instance = this;
//         }
//         private void Start() {
//             SignInToGPGS();
//         }
//         private void OnEnable() {
//             OpenSave(false);
//         }
//         #region Authentication
//         private void SignInToGPGS()
//         {
//             PlayGamesPlatform.Activate();

//             if(!Social.localUser.authenticated)
//             {
//                 PlayGamesPlatform.Instance.Authenticate(SignInCallback);
//             }
//         }
//         private void SignInCallback(SignInStatus status)
//         {
//             if(status == SignInStatus.Success)
//             {
//                 statusTxt.text = "Signed in successfully";
//                 descriptionTxt.text = "Account Name: " + Social.localUser.userName;
//                 achievementButton.SetActive(true);
//                 leaderboardButton.SetActive(true);
//                 savePanelButton.SetActive(true);
//             }
//             else
//             {
//                 statusTxt.text = "Signing in failed...";

//                 // Activate manually signing in
//                 authenticateButton.SetActive(true);
//             }
//         }
//         public void AuthenticateButton()
//         {
//             PlayGamesPlatform.Instance.ManuallyAuthenticate(ManuallyAuthenticateCallback);
//         }
//         private void ManuallyAuthenticateCallback(SignInStatus status)
//         {
//             if(status == SignInStatus.Success)
//             {
//                 statusTxt.text = "Signed in successfully";
//                 descriptionTxt.text = "Account Name: " + Social.localUser.userName;
//                 achievementButton.SetActive(true);
//                 leaderboardButton.SetActive(true);
//                 savePanelButton.SetActive(true);
//                 authenticateButton.SetActive(false);
//             }
//             else
//             {
//                 statusTxt.text = "Signing in failed...";
//             }
//         }
//         #endregion

//         #region SavedGames

//         private bool isSaving;
//         public void OpenSave(bool input) // If input true, activate SAVING otherwise activate LOADING
//         {
//             if(Social.localUser.authenticated)
//             {
//                 isSaving = input;

//                 _savedGamesUI.logTxt.text = "File Opened For Saving/Loading. ";

//                 // Open to file to read data
//                 _savedGamesUI.logTxt.text += Social.localUser.userName + " is authenticated. ";
//                 ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
//                     "MyFileName",
//                     DataSource.ReadCacheOrNetwork,
//                     ConflictResolutionStrategy.UseLongestPlaytime,
//                     SaveOrLoadGameFile
//                 );
//             }
//         }
//         private void SaveOrLoadGameFile(SavedGameRequestStatus status, ISavedGameMetadata meta)
//         {
//             if(status == SavedGameRequestStatus.Success)
//             {
//                 if(isSaving) // Saving
//                 {
//                     _savedGamesUI.logTxt.text += "Saving started... ";
//                     byte[] byteData = System.Text.ASCIIEncoding.ASCII.GetBytes(GetSaveString());

//                     SavedGameMetadataUpdate updateForMetadata = new SavedGameMetadataUpdate.Builder().WithUpdatedDescription("Player Data Updated at " + DateTime.Now.ToString()).Build();

//                     ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(
//                         meta,
//                         updateForMetadata,
//                         byteData,
//                         CallbackSave
//                     );
//                 }
//                 else // Loading
//                 {
//                     _savedGamesUI.logTxt.text += "Loading started... ";
//                     ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(meta, CallbackLoad);
//                 }
//             }
//             else
//             {
//                 _savedGamesUI.logTxt.text += "Unable to load/save... ";
//             }
//         }
//         private void CallbackLoad(SavedGameRequestStatus status, byte[] data)
//         {
//             if(status == SavedGameRequestStatus.Success)
//             {
//                 _savedGamesUI.logTxt.text += "Load successful, trying read the loaded data. ";
//                 string playerDataString = System.Text.ASCIIEncoding.ASCII.GetString(data);

//                 LoadSavedData(playerDataString);
//             }
//             else
//             {
//                 _savedGamesUI.logTxt.text += "Unable to load the data. ";
//             }
//         }
//         private void LoadSavedData(string data)
//         {
//             _savedGamesUI.logTxt.text += "Player Data is readed successfully. ";
//             // var _playerData = JsonUtility.FromJson<PlayerData>(data);
//             // _savedGamesUI.outputTxt.text = "High Score: " + _playerData.highestScore.ToString();
//             string[] playerData = data.Split('-');
//             _savedGamesUI.name = playerData[0];
//             _savedGamesUI.age = int.Parse(playerData[0]);

//             _savedGamesUI.outputTxt.text = "Name: "+ _savedGamesUI.name + ", Age: " + _savedGamesUI.age;
//         }
//         private void CallbackSave(SavedGameRequestStatus status, ISavedGameMetadata meta)
//         {
//             if(status == SavedGameRequestStatus.Success)
//             {
//                 _savedGamesUI.logTxt.text += "Player data saved successfully. ";
//             }
//             else
//             {
//                 _savedGamesUI.logTxt.text += "Player data saving failed. ";
//             }
//         }
//         private string GetSaveString()
//         {
//             // string stringData = JsonUtility.ToJson(_savedGamesUI._playerData);
//             string stringData = _savedGamesUI.name + "-" + _savedGamesUI.age;

//             return stringData;
//         }
//         private void OnDisable() {
//             OpenSave(true);
//         }
//         private void OnApplicationQuit() {
//             OpenSave(true);
//         }
//         #endregion
//     }
// }