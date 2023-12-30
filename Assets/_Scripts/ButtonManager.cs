using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime
{
    public class ButtonManager : MonoBehaviour
    {
        private bool isPaused;
        [SerializeField] private TMP_Text _tcountDown;
        [SerializeField] private InGameUIElements _uiElements;


        public static ButtonManager Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
        }
        public void ShowLeaderboardBTN()
        {
            if(FindObjectOfType<VLeaderboard>() != null)
            {
                VLeaderboard.Instance.ShowLeaderboardUI();

            }
        }
        #region In Game
        public void LoadAScene(string sceneName)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneName);
        }
        public void OpenSoundBTN()
        {
            if(_uiElements.soundButtonAnim != null)
            {
                var slideInBool = _uiElements.soundButtonAnim.GetBool("slideIn");
                slideInBool = slideInBool ? false : true;
                _uiElements.soundButtonAnim.SetBool("slideIn", slideInBool);
            }
        }
        public void PauseButton()
        {
            if(!isPaused)
            {
                if(!_uiElements.endGameMenu.activeInHierarchy)
                {
                    _uiElements.endGameMenu.SetActive(true);
                    _uiElements.endGameUpperBoard.SetActive(true);
                    _uiElements.endGameUpperBoard.GetComponent<Animator>().SetBool("slideDown", true);

                    _uiElements.healthHolder.SetActive(false);
                    _uiElements.scoreHolder.SetActive(false);

                    Time.timeScale = Mathf.Epsilon;
                }
                else
                {
                    _uiElements.endGameUpperBoard.GetComponent<Animator>().SetBool("slideDown", false);
                    isPaused = true;
                    StartCoroutine(nameof(PauseCor));
                }
            }
        }
        private IEnumerator PauseCor()
        {
            float delapsedTime = 3f;
            _tcountDown.gameObject.SetActive(true);
            _tcountDown.text = ((int)delapsedTime).ToString();
            yield return new WaitForSecondsRealtime(1f);
            delapsedTime = 2f;
            _tcountDown.text = ((int)delapsedTime).ToString();
            yield return new WaitForSecondsRealtime(1f);
            delapsedTime = 1f;
            _tcountDown.text = ((int)delapsedTime).ToString();
            yield return new WaitForSecondsRealtime(1f);

            Time.timeScale = 1f;

            _tcountDown.text = "0";
            _tcountDown.gameObject.SetActive(false);
            _uiElements.endGameUpperBoard.SetActive(false);

            _uiElements.endGameMenu.SetActive(false);
            _uiElements.healthHolder.SetActive(true);
            _uiElements.scoreHolder.SetActive(true);
            isPaused = false;

        }
        #endregion
    }

    [System.Serializable]
    public class InGameUIElements{
        public Animator soundButtonAnim;
        public GameObject endGameMenu;
        public GameObject endGameUpperBoard;
        public GameObject healthHolder;
        public GameObject scoreHolder;
    }
}
