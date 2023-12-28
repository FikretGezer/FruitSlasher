using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime
{
    public class VLoginManager : MonoBehaviour
    {
        public GameObject authenticateButton;
        private void Start() {
            SignInToGPGS();
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
                SceneManager.LoadScene(1);
                Debug.Log("Signed in successfully. " + "Account Name: " + Social.localUser.userName);
            }
            else
            {
                Debug.Log("Signing in failed...");

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
                Debug.Log("Signed in successfully. " + "Account Name: " + Social.localUser.userName);
                SceneManager.LoadScene(1);
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
