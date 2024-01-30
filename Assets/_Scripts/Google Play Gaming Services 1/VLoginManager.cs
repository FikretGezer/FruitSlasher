using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using TMPro;
using GooglePlayGames.BasicApi.SavedGame;

namespace Runtime
{
    public class VLoginManager : MonoBehaviour
    {
        public GameObject authenticateButton;
        public TMP_Text _autenhticateT;
        public GameObject _canvas;

        public static SignInStatus Authenticated {get; private set;}
        public static PlayGamesPlatform Platform {get; private set;}
        // private void Awake() {
        //     DontDestroyOnLoad(_canvas);
        //     SignInToGPGS();
        // }
        private void Start() {
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
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
                Debug.Log("Signed in successfully. " + "Account Name: " + Social.localUser.userName);
                _autenhticateT.text = "Signed in successfully. " + "Account Name: " + Social.localUser.userName;
                // SceneManager.LoadScene(1);
            }
            else
            {
                Debug.Log("Signing in failed...");
                _autenhticateT.text = "Signing in failed...";
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
                Debug.Log("Signed in successfully. " + "Account Name: " + Social.localUser.userName);
                // SceneManager.LoadScene(1);
                authenticateButton.SetActive(false);
            }
            else
            {
                Debug.Log("Signing in failed...");
            }
        }
        #endregion

        #region V2 Login
        internal void ProcessAuthentication(SignInStatus status)
        {
            if(status == SignInStatus.Success)
            {
                VGPGSManager.Instance.OpenSavedGame(false);
            }
            else
            {
                VGPGSManager.Instance.Load();
            }
        }

        #endregion
    }
}
