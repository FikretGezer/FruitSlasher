using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using TMPro;

namespace Runtime
{
    public class VLoginManager : MonoBehaviour
    {
        public GameObject authenticateButton;
        public TMP_Text _autenhticateT;
        public GameObject _canvas;


        private void Start() {
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }

        #region Login
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
