using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.Android;
using TMPro;

namespace Runtime
{
    public class VLoginManager : MonoBehaviour
    {
        public GameObject authenticateButton;
        public TMP_Text _autenhticateT;
        public GameObject _canvas;

        public static VLoginManager Instance;
        private void Awake()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) || !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageRead);
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            }
        }

        private void Start() {
            if(Instance == null) Instance = this;
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
        public void ManualAuthenticate()
        {
            if(!PlayGamesPlatform.Instance.IsAuthenticated())
            {
                PlayGamesPlatform.Instance.ManuallyAuthenticate(status => {
                });
            }
        }
        #endregion
    }
}
