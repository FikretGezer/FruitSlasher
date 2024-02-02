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

        public BladeScriptable _selectedBlade;
        public DojoScriptable _selectedDojo { get; private set; }

        [Header("Current Blade UI")]
        [SerializeField] private TMP_Text bladeNameT;
        [SerializeField] private TMP_Text bladeExpT;
        [SerializeField] private Image bladeImage;
        [Header("Current Dojo UI")]
        [SerializeField] private TMP_Text dojoNameT;
        [SerializeField] private TMP_Text dojoExpT;
        [SerializeField] private Image dojoImage;

        public static BladesAndDojos Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
            _playerData = VGPGSManager.Instance._playerData;
        }
        private void OnEnable() {
            SelectADojo();
            SelectABlade();
        }
        private void Start() {
            AreNewBladesOn();
            AreNewDojosOn();
        }
        public void SelectABlade()
        {
            if(_playerData != null)
            {
                if(VGPGSManager.Instance._playerData.unlockedBlades[VGPGSManager.Instance._playerData.currentBladeIndex])
                {
                    _selectedBlade = _bladesHolder.blades[VGPGSManager.Instance._playerData.currentBladeIndex];
                    CurrentBladeUI(_selectedBlade);
                }
            }
        }
        public void SelectADojo()
        {
            if(_playerData != null)
            {
                if(_playerData.unlockedDojos[_playerData.currentDojoIndex])
                {
                    _selectedDojo = _dojosHolder.dojos[_playerData.currentDojoIndex];
                    CurrentDojoUI(_selectedDojo);
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
        private void CurrentBladeUI(BladeScriptable blade)
        {
            if(bladeNameT == null) return;

            bladeNameT.text = blade.bladeName;
            bladeExpT.text = blade.bladeExplanation;
            bladeImage.sprite = blade.bladeSprite;
        }
        private void CurrentDojoUI(DojoScriptable dojo)
        {
            if(dojoNameT == null) return;

            dojoNameT.text = dojo.dojoName;
            dojoExpT.text = dojo.dojoExplanation;
            dojoImage.sprite = dojo.dojoSprite;
        }
    }
}
