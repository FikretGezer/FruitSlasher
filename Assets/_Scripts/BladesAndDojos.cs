using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Runtime
{
    public class BladesAndDojos : MonoBehaviour
    {
        [SerializeField] private BladesHolder _bladesHolder;
        [SerializeField] private DojosHolder _dojosHolder;
        [SerializeField] private Transform _bladesContainerUI;
        [SerializeField] private Sprite _bladeLockedSprite;
        [SerializeField] private Sprite _bladeUnlockedSprite;
        [SerializeField] private SpriteRenderer _dojoRenderer;
        [SerializeField] private GameObject _newBladesText;
        [SerializeField] private GameObject _newDojosText;


        private VPlayerData _playerData;

        public BladeScriptable _selectedBlade;// { get; private set; }
        public DojoScriptable _selectedDojo { get; private set; }

        public static BladesAndDojos Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
            _playerData = VGPGSManager.Instance._playerData;
            SelectABlade();
            SelectADojo();
        }
        private void Start() {
            // if(_isOnMenu)
            // {
            //     UnlockNewBlades();
            // }
            AreNewBladesOn();
            AreNewDojosOn();
            // DefineUnlockedBlades();
        }
        public void SelectABlade()
        {
            if(_playerData != null)
            {
                if(_playerData.unlockedBlades[_playerData.currentBladeIndex])
                {
                    // Debug.Log(_bladesHolder.blades[_playerData.currentBladeIndex].bladeObj.name);
                    _selectedBlade = _bladesHolder.blades[_playerData.currentBladeIndex];
                }
            }
        }
        // private void DefineUnlockedBlades()
        // {
        //     if(_bladesContainerUI != null)
        //     {
        //         for (int i = 0; i < _bladesContainerUI.childCount; i++)
        //         {
        //             if(_playerData.unlockedBlades[i])
        //             {
        //                 _bladesContainerUI.GetChild(i).GetComponent<Image>().sprite = _bladeUnlockedSprite;
        //                 continue;
        //             }
        //             _bladesContainerUI.GetChild(i).GetComponent<Image>().sprite = _bladeLockedSprite;
        //             _bladesContainerUI.GetChild(i).transform.GetChild(0).gameObject.SetActive(false); // Disable Text
        //         }
        //     }
        // }
        // private void UnlockNewBlades()
        // {
        //     if(_playerData.unlockedBlades.Length > _playerData.level)
        //         _playerData.unlockedBlades[_playerData.level] = true;
        // }
        public void SelectADojo()
        {
            if(_playerData != null)
            {
                if(_playerData.unlockedDojos[_playerData.currentDojoIndex])
                {
                    _selectedDojo = _dojosHolder.dojos[_playerData.currentDojoIndex];
                    if(_dojoRenderer != null)
                        _dojoRenderer.sprite = _selectedDojo.dojoSprite;
                }
            }
        }
        private void AreNewBladesOn()
        {
            if(FindObjectOfType<VGPGSManager>() != null)
            {
                if(_playerData.areNewBladesUnlocked)
                {
                    if(_newBladesText != null)
                        _newBladesText.SetActive(true);
                }
            }
        }
        private void AreNewDojosOn()
        {
            if(FindObjectOfType<VGPGSManager>() != null)
            {
                if(_playerData.areNewDojosUnlocked)
                {
                    if(_newDojosText != null)
                        _newDojosText.SetActive(true);
                }
            }
        }
        public void SetInactiveNewBladeText() {
            if(_newBladesText != null)
            {
                _newBladesText.SetActive(false);
                _playerData.areNewBladesUnlocked = false;

            }
        }
        public void SetInactiveNewDojoText()
        {
            if(_newDojosText != null)
            {
                _newDojosText.SetActive(false);
                _playerData.areNewDojosUnlocked = false;
            }
        }
    }
}
