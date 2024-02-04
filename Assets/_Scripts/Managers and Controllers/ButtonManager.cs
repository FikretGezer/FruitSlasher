using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Runtime
{
    public class ButtonManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tcountDown;
        [SerializeField] private InGameUIElements _uiElements;
        [SerializeField] private MenuUIElements _menuUIElements;
        [SerializeField] private StoreUIElements _storeUIElements;
        [SerializeField] private PreGameStoreUIElements _preGameStoreUIElements;
        [SerializeField] private bool isOnMenuStore;
        [SerializeField] private GameObject _missionBoard;
        [Header("Sound Images")]
        [SerializeField] private Image _soundFXImage;
        [SerializeField] private Image _musicImage;
        private float offSetXForDojos = -1f;
        private float offSetXForBlades = 6f;

        private const float aspectRatioForDojos = 16/9f;

        private int selectedBladeIndex; // For Buy Option
        private int selectedDojoIndex; // For Buy Option
        private bool isPaused;

        // Pre Game Store
        private bool didBladesMenuOpen;
        private bool didDojosMenuOpen;
        private GameObject _selectedItem;

        private ItemTypeToBuy _itemTypeToBuy;

        public static ButtonManager Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
        }
        private void Start() {
            SoundMixerManager.Instance.SetStartImages(_soundFXImage, _musicImage);
        }
        public void ShowLeaderboardBTN()
        {
            if(FindObjectOfType<VLeaderboard>() != null)
            {
                VLeaderboard.Instance.ShowLeaderboardUI();
            }
        }
        public void ShowLogs()
        {
            var activeness =  VGPGSManager.Instance._infoCloudT.gameObject.activeInHierarchy;

            VGPGSManager.Instance._infoCloudT.gameObject.SetActive(!activeness);
            VGPGSManager.Instance._infoT.gameObject.SetActive(!activeness);
        }
        public void DeleteGameData()
        {
            VGPGSManager.Instance.DeleteGameData();
            VGPGSManager.Instance.DeleteLocalGameData();
        }
        #region In Game
        public void LoadAScene(string sceneName)
        {
            if(GameManager.Situation == GameSituation.Play || GameManager.Situation == GameSituation.EverythingDone || GameManager.Situation == GameSituation.Paused)
            {
                Time.timeScale = 1f;
                SoundManager.Instance.PlaySFXClip(SoundManager.Instance.Clips.buttonClickSFX);
                if(sceneName == "Menu")
                {
                    SoundManager.Instance.PlayMusic(SoundManager.Instance.Clips.chillMusic);
                }
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
                // SoundManager.Instance.PlaySFXClip(SoundManager.Instance.Clips.soundNMusicButtonSFX);
                SoundManager.Instance.PlaySFXClip(SoundManager.Instance.Clips.swipeSoundsSFX);
            }
        }
        public void PauseButton()
        {
            if(!isPaused)
            {
                SoundManager.Instance.PlaySFXClip(SoundManager.Instance.Clips.buttonClickSFX);
                if(!_uiElements.endGameMenu.activeInHierarchy)
                {
                    _uiElements.endGameMenu.SetActive(true);
                    _uiElements.endGameUpperBoard.SetActive(true);
                    _uiElements.endGameUpperBoard.GetComponent<Animator>().SetBool("slideDown", true);

                    _uiElements.healthHolder.SetActive(false);
                    _uiElements.scoreHolder.SetActive(false);

                    Time.timeScale = Mathf.Epsilon;
                    GameManager.Situation = GameSituation.Paused;
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
            GameManager.Situation = GameSituation.Play;
            isPaused = false;
        }
        #endregion
        #region Menu
        public void ResetAndSave()
        {
            var _playerData = new VPlayerData();
            VGPGSManager.Instance._playerData = _playerData;

        }
        public void ShowAch()
        {
            VAchievement.Instance.ShowAchievements();
        }
        public void SetNewBlade(int index)
        {
            if(VGPGSManager.Instance._playerData.currentBladeIndex != index)
            {
                if(VGPGSManager.Instance._playerData.unlockedBlades[index])
                {
                    VGPGSManager.Instance._playerData.currentBladeIndex = index;
                    BladesAndDojos.Instance.SelectABlade();
                }
            }
        }
        public void OpenStore()
        {
            _menuUIElements.menuUI.SetActive(false);
            _menuUIElements.storeUI.SetActive(true);
            // Store.Instance.CheckUnlockedBlades();
            // Store.Instance.CheckUnlockedDojos();
            Store.Instance.ResetPositionOfScroll();
            SoundManager.Instance.PlaySFXClip(SoundManager.Instance.Clips.swipeMenusSFX);

            if(VGPGSManager.Instance._playerData.areNewBladesUnlocked)
                VGPGSManager.Instance._playerData.areNewBladesUnlocked = false;
        }
        public void GetBackStore()
        {
            _menuUIElements.menuUI.SetActive(true);
            _menuUIElements.storeUI.SetActive(false);
            _storeUIElements.container.SetActive(false);
            SoundManager.Instance.PlaySFXClip(SoundManager.Instance.Clips.buttonClickSFX);
        }
        #endregion
        #region Menu Store
        public void StoreOpenBlades()
        {
            if(!_storeUIElements.bladesUI.activeInHierarchy)
            {
                _storeUIElements.bladesUI.SetActive(true);
                _storeUIElements.dojosUI.SetActive(false);
                Store.Instance.ResetPositionOfScroll();
            }
        }
        public void StoreOpenDojos()
        {
            if(!_storeUIElements.dojosUI.activeInHierarchy)
            {
                _storeUIElements.dojosUI.SetActive(true);
                _storeUIElements.bladesUI.SetActive(false);
                Store.Instance.ResetPositionOfScroll();

                if(VGPGSManager.Instance._playerData.areNewDojosUnlocked)
                    VGPGSManager.Instance._playerData.areNewDojosUnlocked = false;
            }
        }
        public void SetItemBlade(int index)
        {
            VPlayerData _playerData = VGPGSManager.Instance._playerData;

            // BladeScriptable currentBlade = BladesAndDojos.Instance._selectedBlade;
            BladeScriptable currentBlade = _storeUIElements.bladeHolder.blades[index];

            _storeUIElements.itemImage.sprite = currentBlade.bladeSprite;
            _storeUIElements.itemImage.GetComponent<AspectRatioFitter>().aspectRatio = 1f;

            if(_playerData.unlockedBlades[index])
            {
                //Set Explanation Box
                _storeUIElements.itemLockedImage.SetActive(false);
                _storeUIElements.container.SetActive(true);
                _storeUIElements.itemName.text = currentBlade.bladeName;
                _storeUIElements.itemExp.text = currentBlade.bladeExplanation;

                // Set Actual Dojo
                if(_playerData.boughtBlades[index])
                {
                    _playerData.currentBladeIndex = index;
                    BladesAndDojos.Instance.SelectABlade();


                    _storeUIElements.itemBuyButton.GetComponent<Image>().sprite = _storeUIElements.boughtSprite;
                    _storeUIElements.itemBuyText.text = "BOUGHT";
                    _storeUIElements.itemBuyButton.GetComponent<Button>().enabled = false;
                    _storeUIElements.itemPriceContainer.SetActive(false);
                    VGPGSManager.Instance.SaveDouble();
                }
                else
                {
                    _storeUIElements.itemBuyButton.GetComponent<Image>().sprite = _storeUIElements.buySprite;
                    _storeUIElements.itemBuyButton.GetComponent<Button>().enabled = true;
                    _storeUIElements.itemBuyButton.gameObject.SetActive(true);

                    _storeUIElements.itemPriceContainer.SetActive(true);
                    _storeUIElements.itemPrice.text = currentBlade.bladePrice.ToString();

                    _storeUIElements.itemBuyText.text = "BUY";
                    selectedBladeIndex = index;
                    _itemTypeToBuy = ItemTypeToBuy.Blade;
                }
            }
            else
            {
                _storeUIElements.container.SetActive(true);
                _storeUIElements.itemName.text = "Locked Blade";
                _storeUIElements.itemExp.text = $"This blade will be unlocked at level {currentBlade.bladeLevel}";

                _storeUIElements.itemBuyButton.gameObject.SetActive(false);
                _storeUIElements.itemPriceContainer.SetActive(false);
                _storeUIElements.itemLockedImage.SetActive(true);
            }
        }
        public void SetItemDojo(int index)
        {
            VPlayerData _playerData = VGPGSManager.Instance._playerData;

            // DojoScriptable currentDojo = BladesAndDojos.Instance._selectedDojo;
            DojoScriptable currentDojo = _storeUIElements.dojoHolder.dojos[index];

            _storeUIElements.itemImage.sprite = currentDojo.dojoSprite;
            _storeUIElements.itemImage.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatioForDojos;

            if(_playerData.unlockedDojos[index])
            {
                //Set Explanation Box
                _storeUIElements.itemLockedImage.SetActive(false);
                _storeUIElements.container.SetActive(true);
                _storeUIElements.itemName.text = currentDojo.dojoName;
                _storeUIElements.itemExp.text = currentDojo.dojoExplanation;

                // Set Actual Dojo
                if(_playerData.boughtDojos[index])
                {
                    _playerData.currentDojoIndex = index;
                    BladesAndDojos.Instance.SelectADojo();


                    _storeUIElements.itemBuyButton.GetComponent<Image>().sprite = _storeUIElements.boughtSprite;
                    _storeUIElements.itemBuyText.text = "BOUGHT";
                    _storeUIElements.itemBuyButton.GetComponent<Button>().enabled = false;
                    _storeUIElements.itemPriceContainer.SetActive(false);
                    Background.Instance.FitBgToScreen();
                    VGPGSManager.Instance.SaveDouble();
                }
                else
                {
                    _storeUIElements.itemBuyButton.GetComponent<Image>().sprite = _storeUIElements.buySprite;
                    _storeUIElements.itemBuyButton.GetComponent<Button>().enabled = true;
                    _storeUIElements.itemBuyButton.gameObject.SetActive(true);

                    _storeUIElements.itemPriceContainer.SetActive(true);
                    _storeUIElements.itemPrice.text = currentDojo.dojoPrice.ToString();

                    _storeUIElements.itemBuyText.text = "BUY";

                    selectedDojoIndex = index;
                    _itemTypeToBuy = ItemTypeToBuy.Dojo;
                }
            }
            else
            {
                _storeUIElements.container.SetActive(true);
                _storeUIElements.itemName.text = "Locked Dojo";
                _storeUIElements.itemExp.text = $"This dojo will be unlocked at level {currentDojo.dojoLevel}";

                _storeUIElements.itemBuyButton.gameObject.SetActive(false);
                _storeUIElements.itemPriceContainer.SetActive(false);
                _storeUIElements.itemLockedImage.SetActive(true);
            }
        }
        public void StoreBuyItem()
        {
            var _playerData = VGPGSManager.Instance._playerData;
            if(_itemTypeToBuy == ItemTypeToBuy.Dojo)
            {
                var price = _storeUIElements.dojoHolder.dojos[selectedDojoIndex].dojoPrice;

                if(_playerData.stars >= price)
                {
                    _playerData.stars -= price;
                    _playerData.boughtDojos[selectedDojoIndex] = true;

                    _playerData.currentDojoIndex = selectedDojoIndex;
                    BladesAndDojos.Instance.SelectADojo();
                    MenuUI.Instance.SetStars();
                    SoundManager.Instance.PlaySFXClip(SoundManager.Instance.Clips.itemBuySFX);

                    if(FindObjectOfType<MissionController>())
                    {
                        foreach(var mission in MissionController.Instance._selectedMissionsScriptable.selectedMissions)
                        {
                            if(mission.type == MissionType.BuyDojo)
                            {
                                mission.BuyDojo();
                            }
                        }
                    }

                    _storeUIElements.itemBuyButton.GetComponent<Image>().sprite = _storeUIElements.boughtSprite;
                    _storeUIElements.itemBuyText.text = "BOUGHT";
                    _storeUIElements.itemBuyButton.GetComponent<Button>().enabled = false;
                    _storeUIElements.itemPriceContainer.SetActive(false);
                    Background.Instance.FitBgToScreen();
                    VGPGSManager.Instance.SaveDouble();
                }
            }
            else if(_itemTypeToBuy == ItemTypeToBuy.Blade)
            {
                var price = _storeUIElements.bladeHolder.blades[selectedBladeIndex].bladePrice;

                if(_playerData.stars >= price)
                {
                    _playerData.stars -= price;
                    _playerData.boughtBlades[selectedBladeIndex] = true;

                    _playerData.currentBladeIndex = selectedBladeIndex;
                    BladesAndDojos.Instance.SelectABlade();
                    MenuUI.Instance.SetStars();
                    SoundManager.Instance.PlaySFXClip(SoundManager.Instance.Clips.itemBuySFX);

                    if(FindObjectOfType<MissionController>())
                    {
                        foreach(var mission in MissionController.Instance._selectedMissionsScriptable.selectedMissions)
                        {
                            if(mission.type == MissionType.BuyBlade)
                            {
                                mission.BuyBlade();
                            }
                        }
                    }

                    _storeUIElements.itemBuyButton.GetComponent<Image>().sprite = _storeUIElements.boughtSprite;
                    _storeUIElements.itemBuyText.text = "BOUGHT";
                    _storeUIElements.itemBuyButton.GetComponent<Button>().enabled = false;
                    _storeUIElements.itemPriceContainer.SetActive(false);
                    VGPGSManager.Instance.SaveDouble();
                }
            }
            _itemTypeToBuy = ItemTypeToBuy.None;
        }
        #endregion
        #region Pre Game Store
        public void PreGameNewBlades()
        {
            BladesAndDojos.Instance.SetInactiveNewBladeText();
            OpenPreGameBladesMenu();
        }
        public void PreGameNewDojos()
        {
            BladesAndDojos.Instance.SetInactiveNewDojoText();
            OpenPreGameDojosMenu();
        }
        private void OpenPreGameBladesMenu()
        {
            didBladesMenuOpen = !didBladesMenuOpen;
            _preGameStoreUIElements.bladesAnimator.SetBool("openBladesMenu", didBladesMenuOpen);
            SoundManager.Instance.PlaySFXClip(SoundManager.Instance.Clips.swipeMenusSFX);
        }
        private void OpenPreGameDojosMenu()
        {
            didDojosMenuOpen = !didDojosMenuOpen;
            _preGameStoreUIElements.dojosAnimator.SetBool("openDojosMenu", didDojosMenuOpen);
            SoundManager.Instance.PlaySFXClip(SoundManager.Instance.Clips.swipeMenusSFX);

            if(didDojosMenuOpen)
                _preGameStoreUIElements.playButton.SetActive(false);
            else
                _preGameStoreUIElements.playButton.SetActive(true);
        }
        public void SetPreGameItemBlade(GameObject priceContainer, Transform t, int index)
        {
            VPlayerData _playerData = VGPGSManager.Instance._playerData;
            if(_playerData.unlockedBlades[index])
            {
                BladeScriptable currentBlade = _storeUIElements.bladeHolder.blades[index];

                // Set Actual Dojo
                if(_playerData.boughtBlades[index])
                {
                    _playerData.currentBladeIndex = index;
                    BladesAndDojos.Instance.SelectABlade();


                    _preGameStoreUIElements.bladeBuyButton.gameObject.SetActive(false);
                    VGPGSManager.Instance.SaveDouble();
                }
                else
                {
                    var pos = t.position;
                    pos.x += offSetXForBlades;
                    _preGameStoreUIElements.bladeBuyButton.position = pos;
                    _preGameStoreUIElements.bladeBuyButton.gameObject.SetActive(true);

                    selectedBladeIndex = index;
                    _selectedItem = priceContainer;
                    _itemTypeToBuy = ItemTypeToBuy.Blade;
                }
            }
            else
            {
                _preGameStoreUIElements.bladeBuyButton.gameObject.SetActive(false);
            }
        }
        public void SetPreGameItemDojo(GameObject priceContainer, Transform t, int index)
        {
            VPlayerData _playerData = VGPGSManager.Instance._playerData;
            if(_playerData.unlockedDojos[index])
            {
                DojoScriptable currentDojo = _storeUIElements.dojoHolder.dojos[index];

                // Set Actual Dojo
                if(_playerData.boughtDojos[index])
                {
                    _playerData.currentDojoIndex = index;
                    BladesAndDojos.Instance.SelectADojo();

                    Background.Instance.FitBgToScreen();

                    _preGameStoreUIElements.dojoBuyButton.gameObject.SetActive(false);
                    VGPGSManager.Instance.SaveDouble();
                }
                else
                {
                    var pos = t.position;
                    pos.x += offSetXForDojos;
                    _preGameStoreUIElements.dojoBuyButton.position = pos;
                    _preGameStoreUIElements.dojoBuyButton.gameObject.SetActive(true);

                    selectedDojoIndex = index;
                    _selectedItem = priceContainer;
                    _itemTypeToBuy = ItemTypeToBuy.Dojo;
                }
            }
            else
            {
                _preGameStoreUIElements.dojoBuyButton.gameObject.SetActive(false);
            }
        }
        public void PreGameStoreBuyItem()
        {
            var _playerData = VGPGSManager.Instance._playerData;
            if(_itemTypeToBuy == ItemTypeToBuy.Dojo)
            {
                var price = _storeUIElements.dojoHolder.dojos[selectedDojoIndex].dojoPrice;

                if(_playerData.stars >= price)
                {
                    _playerData.stars -= price;
                    _playerData.boughtDojos[selectedDojoIndex] = true;

                    _playerData.currentDojoIndex = selectedDojoIndex;
                    BladesAndDojos.Instance.SelectADojo();
                    MenuUI.Instance.SetStars();
                    SoundManager.Instance.PlaySFXClip(SoundManager.Instance.Clips.itemBuySFX);

                    Background.Instance.FitBgToScreen();

                    if(_selectedItem != null)
                    {
                        _selectedItem.SetActive(false);
                        _selectedItem = null;
                    }

                    if(FindObjectOfType<MissionController>())
                    {
                        foreach(var mission in MissionController.Instance._selectedMissionsScriptable.selectedMissions)
                        {
                            if(mission.type == MissionType.BuyDojo)
                            {
                                mission.BuyDojo();
                            }
                        }
                    }

                    _preGameStoreUIElements.dojoBuyButton.gameObject.SetActive(false);
                    VGPGSManager.Instance.SaveDouble();
                }
            }
            else if(_itemTypeToBuy == ItemTypeToBuy.Blade)
            {
                var price = _storeUIElements.bladeHolder.blades[selectedBladeIndex].bladePrice;

                if(_playerData.stars >= price)
                {
                    _playerData.stars -= price;
                    _playerData.boughtBlades[selectedBladeIndex] = true;

                    _playerData.currentBladeIndex = selectedBladeIndex;
                    BladesAndDojos.Instance.SelectABlade();
                    MenuUI.Instance.SetStars();
                    SoundManager.Instance.PlaySFXClip(SoundManager.Instance.Clips.itemBuySFX);


                    if(_selectedItem != null)
                    {
                        _selectedItem.SetActive(false);
                        _selectedItem = null;
                    }

                    if(FindObjectOfType<MissionController>())
                    {
                        foreach(var mission in MissionController.Instance._selectedMissionsScriptable.selectedMissions)
                        {
                            if(mission.type == MissionType.BuyBlade)
                            {
                                mission.BuyBlade();
                            }
                        }
                    }

                    _preGameStoreUIElements.bladeBuyButton.gameObject.SetActive(false);
                    VGPGSManager.Instance.SaveDouble();
                }
            }
            _itemTypeToBuy = ItemTypeToBuy.None;
        }
        public void OpenMissionBoard(Animator animator)
        {
            var res = animator.GetBool("slideIn");
            animator.SetBool("slideIn", !res);
            SoundManager.Instance.PlaySFXClip(SoundManager.Instance.Clips.swipeMenusSFX);
            // var activeness = !_missionBoard.activeInHierarchy;
            // _missionBoard.SetActive(activeness);
        }
        #endregion
        #region Sound Buttons
        public void MusicOnAndOff(){
            SoundMixerManager.Instance.SetMusicVolume(_musicImage);
            VGPGSManager.Instance.SaveDouble();
        }
        public void SoundFXOnAndOff(){
            SoundMixerManager.Instance.SetSoundFXVolume(_soundFXImage);
            VGPGSManager.Instance.SaveDouble();
        }

        #endregion
    }
}
