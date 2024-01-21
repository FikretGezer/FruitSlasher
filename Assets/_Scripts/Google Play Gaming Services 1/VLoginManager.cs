using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Runtime
{
    public class VLoginManager : MonoBehaviour
    {
        public GameObject authenticateButton;
        public TMP_Text _autenhticateT;
        public GameObject _canvas;
        private void Awake() {
            DontDestroyOnLoad(_canvas);
            SignInToGPGS();
        }
        // private void Start() {
        // }
        #region Authentication
        private void SignInToGPGS()
        {

            if(!Social.localUser.authenticated)
            {
                PlayGamesPlatform.Instance.Authenticate(SignInCallback);
            }
                PlayGamesPlatform.Activate();
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
                // SceneManager.LoadScene(1);

                // Activate manually signing in
                // authenticateButton.SetActive(true);
                // SignInToGPGS();
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
    }
}
