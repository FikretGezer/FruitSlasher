using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Runtime
{
    public class ButtonManager : MonoBehaviour
    {
        private bool isPaused;
        [SerializeField] private TMP_Text _tcountDown;
        [SerializeField] private InGameUIElements _uiElements;
        [SerializeField] private MenuUIElements _menuUIElements;
        [SerializeField] private StoreUIElements _storeUIElements;


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
            if(GameManager.Situation == GameSituation.Play || GameManager.Situation == GameSituation.EverythingDone)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(sceneName);
            }
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
        #region Menu
        public void ResetAndSave()
        {
            var _playerData = new VPlayerData();
            VGPGSManager.Instance._playerData = _playerData;
            VGPGSManager.Instance.OpenSave(true);
        }
        public void LoadSave()
        {
            VGPGSManager.Instance.OpenSave(false);
        }
        public void SetNewBlade(int index)
        {
            if(VGPGSManager.Instance._playerData.currentBladeIndex != index)
            {
                if(VGPGSManager.Instance._playerData.unlockedBlades[index])
                {
                    VGPGSManager.Instance._playerData.currentBladeIndex = index;
                    BladesAndDojos.Instance.SelectABlade();
                    VGPGSManager.Instance.OpenSave(true);
                }
            }
        }
        public void OpenStore()
        {
            _menuUIElements.menuUI.SetActive(false);
            _menuUIElements.storeUI.SetActive(true);
        }
        public void GetBackStore()
        {
            _menuUIElements.menuUI.SetActive(true);
            _menuUIElements.storeUI.SetActive(false);
            _storeUIElements.container.SetActive(false);
        }
        #endregion
        #region Store
        public void StoreOpenBlades()
        {
            _storeUIElements.bladesUI.SetActive(true);
            _storeUIElements.dojosUI.SetActive(false);
        }
        public void StoreOpenDojos()
        {
            _storeUIElements.dojosUI.SetActive(true);
            _storeUIElements.bladesUI.SetActive(false);
        }
        public void SetItemBlade(int index)
        {
            VPlayerData _playerData = VGPGSManager.Instance._playerData;
            if(_playerData.unlockedBlades[index])
            {
                _playerData.currentBladeIndex = index;
                BladesAndDojos.Instance.SelectABlade();
                VGPGSManager.Instance.OpenSave(true);

                BladeScriptable currentBlade = BladesAndDojos.Instance._selectedBlade;
                _storeUIElements.container.SetActive(true);
                _storeUIElements.itemImage.sprite = currentBlade.bladeSprite;
                _storeUIElements.itemName.text = currentBlade.bladeName;
                _storeUIElements.itemExp.text = currentBlade.bladeExplanation;
            }
        }
        public void SetItemDojo(int index)
        {
            VPlayerData _playerData = VGPGSManager.Instance._playerData;
            if(_playerData.unlockedDojos[index])
            {
                _playerData.currentDojoIndex = index;
                BladesAndDojos.Instance.SelectADojo();
                VGPGSManager.Instance.OpenSave(true);

                DojoScriptable currentDojo = BladesAndDojos.Instance._selectedDojo;
                _storeUIElements.container.SetActive(true);
                _storeUIElements.itemImage.sprite = currentDojo.dojoSprite;
                _storeUIElements.itemName.text = currentDojo.dojoName;
                _storeUIElements.itemExp.text = currentDojo.dojoExplanation;//
            }
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
    [System.Serializable]
    public class MenuUIElements{
        public GameObject menuUI;
        public GameObject storeUI;
    }
    [System.Serializable]
    public class StoreUIElements{
        public GameObject bladesUI;
        public GameObject dojosUI;
        [Header("Item Explanation Container")]
        public Image itemImage;
        public TMP_Text itemName;
        public TMP_Text itemExp;
        public GameObject container;

    }
}
