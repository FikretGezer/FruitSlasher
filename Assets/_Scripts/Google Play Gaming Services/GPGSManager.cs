using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using TMPro;
using System;

[System.Serializable]
public class PlayerData {
    public int highestScore;
    public int level;
    public int experience;
    public bool[] achievements;
    public bool[] blades;
    public bool[] backgrounds;
}
public class GPGSManager : MonoBehaviour
{
    // #region Default GPGS Save and Load

    // public TMP_Text statusTxt;
    // public TMP_Text descriptionTxt;
    // public GameObject achievementButton;
    // public GameObject leaderboardButton;
    // public SavedGamesUI _savedGamesUI;

    // private void Start() {
    //     SignInToGPGS();
    // }
    // internal void SignInToGPGS()
    // {
    //     PlayGamesPlatform.Activate();
    //     PlayGamesPlatform.Instance.Authenticate(code => {
    //         statusTxt.text = "Autenticating...";
    //         if(code == SignInStatus.Success)
    //         {
    //             statusTxt.text = "Successfully Authenticated.";
    //             descriptionTxt.text = "Hello " + Social.localUser.userName + ". You have an ID of " + Social.localUser.id;
    //             achievementButton.SetActive(true);
    //             leaderboardButton.SetActive(true);
    //         }
    //         else{
    //             statusTxt.text = "Failed to Authenticate";
    //             descriptionTxt.text = "Failed to Authenticate, reason for failure is " + code;
    //         }
    //     });
    // }

    // #region SavedGames
    // private bool isSaving;
    // public void OpenSave(bool _isSaving)
    // {
    //     _savedGamesUI.logTxt.text += "\n";
    //     _savedGamesUI.logTxt.text += "Open Saved Clicked\n";
    //     if(Social.localUser.authenticated)
    //     {
    //         this.isSaving = _isSaving;
    //         _savedGamesUI.logTxt.text += "User is authenticated\n";
    //         ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
    //             "MyFileName",
    //             DataSource.ReadCacheOrNetwork,
    //             ConflictResolutionStrategy.UseLongestPlaytime,
    //             SaveGameOpen
    //         );
    //     }
    // }
    // private void SaveGameOpen(SavedGameRequestStatus _status, ISavedGameMetadata _metaData)
    // {
    //     if(_status == SavedGameRequestStatus.Success)
    //     {
    //         if(isSaving) // Saving
    //         {
    //             // covert datatypes to a byte array
    //             _savedGamesUI.logTxt.text += "Status successful, attempting to save...\n";
    //             byte[] myData = System.Text.ASCIIEncoding.ASCII.GetBytes(GetSaveString());

    //             // update metadata
    //             SavedGameMetadataUpdate UpdateForMetaData = new SavedGameMetadataUpdate.Builder()
    //             .WithUpdatedDescription( "I have updated my game at: " + DateTime.Now.ToString()).Build();

    //             // commit the save
    //             ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(
    //                 _metaData,
    //                 UpdateForMetaData,
    //                 myData,
    //                 SaveCallBack
    //             );
    //         }
    //         else // Loading
    //         {
    //             _savedGamesUI.logTxt.text += "Status successful, attempting to load...\n";
    //             ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(_metaData, LoadCallBack);
    //         }
    //     }
    // }
    // private void LoadCallBack(SavedGameRequestStatus _status, byte[] _data)
    // {
    //     if(_status == SavedGameRequestStatus.Success)
    //     {
    //         _savedGamesUI.logTxt.text += "Load successful, attemting to read the data\n";
    //         string loadedData = System.Text.ASCIIEncoding.ASCII.GetString(_data);

    //         LoadSavedString(loadedData);
    //     }
    // }
    // public void LoadSavedString(string _cloudData)
    // {
    //     string[] cloudStringArr = _cloudData.Split('|');

    //     _savedGamesUI.name = cloudStringArr[0];
    //     _savedGamesUI.age = int.Parse(cloudStringArr[1]);

    //     _savedGamesUI.outputTxt.text = "";
    //     _savedGamesUI.outputTxt.text = "Name: " + _savedGamesUI.name + ", Age: " + _savedGamesUI.age;
    // }
    // public string GetSaveString()
    // {
    //     string dataToSave = "";

    //     dataToSave += _savedGamesUI.name;
    //     dataToSave += "|" + _savedGamesUI.age;

    //     return dataToSave;
    // }
    // private void SaveCallBack(SavedGameRequestStatus _status, ISavedGameMetadata _metaData)
    // {
    //     if(_status == SavedGameRequestStatus.Success)
    //     {
    //         _savedGamesUI.logTxt.text += "File successfully saved to the cloud...\n";
    //     }
    //     else
    //     {
    //         _savedGamesUI.logTxt.text += "File failed to saved to the cloud...\n";
    //     }
    // }
    // #endregion

    // #endregion

    private TMP_Text descriptionTxt;
    private PlayerData _playerData;
    private string dataKey = "player_data";
    private void Start()
    {
        SignInToGPGS();
    }
    private void SignInToGPGS()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(status => {
            if(status == SignInStatus.Success)
            {
                Debug.Log("Signed in");
            }
            else
                Debug.Log("Signed in failed...");
        });
    }
    #region Saving
    public void SavePlayerDataToCloud()
    {
        if(Social.localUser.authenticated)
        {
            string jsonData = JsonUtility.ToJson(_playerData);
            SaveDataToCloud(dataKey, jsonData);
        }
    }
    private void SaveDataToCloud(string key, string data)
    {
        ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
            key,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            (status, metadata) => OnSaveGameOpenForWriting(status, metadata, data)
        );
    }
    private void OnSaveGameOpenForWriting(SavedGameRequestStatus status, ISavedGameMetadata metadata, string data)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(data);
            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
            builder.WithUpdatedDescription("Data saved at " + System.DateTime.Now);

            ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(
                metadata,
                builder.Build(),
                bytes,
                OnSavedGameWritten
            );
        }
    }
    private void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata metadata)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Data saved successfully");
        }
    }
    #endregion
    #region Loading
    public void LoadPlayerDataFromCloud()
    {
        if(Social.localUser.authenticated)
        {
            LoadDataFromCloud(dataKey, OnDataLoaded);
        }
    }
    private void LoadDataFromCloud(string key, Action<string> onDataLoaded)
    {
        ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
            key,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            (status, metadata) => OnSaveGameOpenForReading(status, metadata, onDataLoaded)
        );
    }
    private void OnSaveGameOpenForReading(SavedGameRequestStatus status, ISavedGameMetadata metadata, Action<string> onDataLoaded)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(
                metadata,
                (loadStatus, bytes) => OnSavedGameDataLoaded(loadStatus, bytes, onDataLoaded)
            );
        }
    }
    private void OnDataLoaded(string jsonData)
    {
        // Deserialize the data
        _playerData = JsonUtility.FromJson<PlayerData>(jsonData);

        descriptionTxt.text = $"Highest Score: {_playerData.highestScore}"+
                              $"Level : {_playerData.level}"+
                              $"Experience: {_playerData.experience}";
    }
    private void OnSavedGameDataLoaded(SavedGameRequestStatus status, byte[] bytes, Action<string> onDataLoaded)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            string jsonData = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
            onDataLoaded?.Invoke(jsonData);
        }
    }
    #endregion
}

/*
public PlayerData _playerData;

    public TMP_Text statusTxt;
    public TMP_Text descriptionTxt;
    public GameObject achievementButton;
    public GameObject leaderboardButton;
    private string dataKey = "player_data";

    private void Start() {
        SignInToGPGS();
    }
    internal void SignInToGPGS()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(code => {
            statusTxt.text = "Autenticating...";
            if(code == SignInStatus.Success)
            {
                statusTxt.text = "Successfully Authenticated.";
                descriptionTxt.text = "Hello " + Social.localUser.userName + ". You have an ID of " + Social.localUser.id;
                achievementButton.SetActive(true);
                leaderboardButton.SetActive(true);
            }
            else{
                statusTxt.text = "Failed to Authenticate";
                descriptionTxt.text = "Failed to Authenticate, reason for failure is " + code;
            }
        });
    }
    #region Saving
    public void SaveThePlayerDataToCloud()
    {
        // Serialize the data
        string jsonData = JsonUtility.ToJson(_playerData);
        SaveDataToTheCloud(dataKey, jsonData);
    }
    private void SaveDataToTheCloud(string key, string data)
    {
        ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
            key,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            (status, metadata) => OnSaveGameOpenForWriting(status, metadata, data)
        );
    }
    private void OnSaveGameOpenForWriting(SavedGameRequestStatus status, ISavedGameMetadata metadata, string data)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(data);
            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
            builder.WithUpdatedDescription("Saved game at " + System.DateTime.Now);

            ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(
                metadata,
                builder.Build(),
                bytes,
                OnSavedGameWritten
            );
        }
    }
    private void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata metadata)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Game data saved to the cloud");
        }
    }
    #endregion
    #region Loading
    public void LoadThePlayerDataFromCloud()
    {
        if(Social.localUser.authenticated)
        {
            LoadDataFromCloud(dataKey, OnDataLoaded);
        }
    }
    private void LoadDataFromCloud(string key, Action<string> onDataLoaded)
    {
        ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
            key,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            (status, metadata) => OnSaveGameOpenForReading(status, metadata, onDataLoaded)
        );
    }
    private void OnSaveGameOpenForReading(SavedGameRequestStatus status, ISavedGameMetadata metadata, Action<string> onDataLoaded)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(
                metadata,
                (loadStatus, bytes) => {
                    OnSavedGameDataLoaded(loadStatus, bytes, onDataLoaded);
                }
            );
        }
    }
    private void OnDataLoaded(string jsonData)
    {
        // Deserialize the data
        _playerData = JsonUtility.FromJson<PlayerData>(jsonData);

        descriptionTxt.text = $"Highest Score: {_playerData.highestScore}"+
                              $"Level : {_playerData.level}"+
                              $"Experience: {_playerData.experience}";
    }
    private void OnSavedGameDataLoaded(SavedGameRequestStatus status, byte[] bytes, Action<string> onDataLoaded)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            string jsonData = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
            onDataLoaded?.Invoke(jsonData);
        }
    }
    #endregion
*/
// public void SaveClassToCloud(bool _isSaving)
    // {
    //     if(Social.localUser.authenticated)
    //     {
    //         isSaving = _isSaving;
    //         string jsonData = JsonUtility.ToJson(_playerData);
    //         SaveDataToCloud("player_data", jsonData);
    //     }
    // }
    // private void SaveDataToCloud(string key, string data)
    // {
    //     ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(key,
    //         DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime,
    //         (status, metadata) => OnSaveGameOpenedForWriting(status, metadata, data));
    // }
    // private void OnSaveGameOpenedForWriting(SavedGameRequestStatus status, ISavedGameMetadata game, string data)
    // {
    //     if (status == SavedGameRequestStatus.Success)
    //     {
    //         byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(data);
    //         SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
    //         builder = builder.WithUpdatedDescription("Saved game at " + System.DateTime.Now);

    //         ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
    //         savedGameClient.CommitUpdate(game, builder.Build(), bytes, OnSavedGameWritten);
    //     }
    //     else
    //     {
    //         Debug.LogError("Failed to open saved game for writing.");
    //     }
    // }
    // private void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    // {
    //     if (status == SavedGameRequestStatus.Success)
    //     {
    //         Debug.Log("Game data saved to the cloud.");
    //     }
    //     else
    //     {
    //         Debug.LogError("Failed to write saved game data.");
    //     }
    // }
    // #endregion

    // #region Loading
    // public void LoadClassFromCloud()
    // {
    //     if (Social.localUser.authenticated)
    //     {
    //         // Load the JSON string from the cloud
    //         LoadDataFromCloud("player_data", OnClassDataLoaded);
    //     }
    // }
    // private void LoadDataFromCloud(string key, Action<string> onDataLoaded)
    // {
    //     ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(key,
    //         DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime,
    //         (status, metadata) => OnSaveGameOpenedForReading(status, metadata, onDataLoaded));
    // }
    // private void OnClassDataLoaded(string jsonData)
    // {
    //     // Deserialize JSON string back into class instance
    //     _playerData = JsonUtility.FromJson<PlayerData>(jsonData);

    //     // Use the loaded class instance as needed
    //     Debug.Log("Loaded Player Data - Score: " + _playerData.highestScore + ", Experience: " + _playerData.experience);
    // }
    // private void OnSaveGameOpenedForReading(SavedGameRequestStatus status, ISavedGameMetadata game, Action<string> onDataLoaded)
    // {
    //     if (status == SavedGameRequestStatus.Success)
    //     {
    //         ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, (loadStatus, bytes) => OnSavedGameDataLoaded(loadStatus, bytes, onDataLoaded));
    //     }
    //     else
    //     {
    //         Debug.LogError("Failed to open saved game for reading.");
    //     }
    // }
    // private void OnSavedGameDataLoaded(SavedGameRequestStatus status, byte[] bytes, Action<string> onDataLoaded)
    // {
    //     if (status == SavedGameRequestStatus.Success)
    //     {
    //         string jsonData = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
    //         onDataLoaded?.Invoke(jsonData);
    //     }
    //     else
    //     {
    //         Debug.LogError("Failed to load saved game data.");
    //     }
    // }
