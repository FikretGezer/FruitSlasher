using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Runtime
{
    public class PreGameStore : MonoBehaviour
    {
        [Header("Pre Game")]
        [SerializeField] private BladesHolder _bladesHolder;
        [SerializeField] private DojosHolder _dojosHolder;
        [SerializeField] private GameObject _preGameItemBoxPrefab;
        [SerializeField] private Transform _bladesContainer;
        [SerializeField] private Transform _dojosContainer;

        private List<GameObject> bladesObjList = new List<GameObject>();
        private List<GameObject> dojosObjList = new List<GameObject>();

        public static PreGameStore Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
        }
        private void OnEnable() {
            CheckUnlockedBlades();
            CheckUnlockedDojos();
            AddBlades();
            AddDojos();
        }
        private void AddBlades()
        {
            for (int i = 0; i < _bladesHolder.blades.Length; i++)
            {
                int current = i;
                var _blade = _bladesHolder.blades[i];
                var newItem = Instantiate(_preGameItemBoxPrefab);

                newItem.transform.GetChild(0).GetComponent<Image>().sprite = _blade.bladeSprite;
                newItem.transform.GetChild(3).transform.GetChild(0).GetComponent<TMP_Text>().text = _blade.bladeName;
                newItem.transform.GetChild(3).transform.GetChild(1).GetComponent<TMP_Text>().text = _blade.bladeExplanation;

                if(!VGPGSManager.Instance._playerData.unlockedBlades[current])
                {
                    newItem.transform.GetChild(2).gameObject.SetActive(true);
                    newItem.transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = _bladesHolder.blades[i].bladeLevel.ToString() + "\nLevel";
                }
                else if(!VGPGSManager.Instance._playerData.boughtBlades[current])
                {
                    newItem.transform.GetChild(2).gameObject.SetActive(false);
                    newItem.transform.GetChild(3).transform.GetChild(2).gameObject.SetActive(true);
                    newItem.transform.GetChild(3).transform.GetChild(2).transform.GetChild(1).GetComponent<TMP_Text>().text = _blade.bladePrice.ToString();
                }

                newItem.transform.SetParent(_bladesContainer);
                newItem.transform.localScale = Vector3.one;
                newItem.GetComponent<Button>().onClick.AddListener(() => ButtonManager.Instance.SetPreGameItemBlade(newItem.transform.GetChild(3).transform.GetChild(2).gameObject, newItem.transform, current));

                bladesObjList.Add(newItem);
            }
        }
        private void AddDojos()
        {
            for (int i = 0; i < _dojosHolder.dojos.Length; i++)
            {
                int current = i;
                var _dojo = _dojosHolder.dojos[i];
                var newItem = Instantiate(_preGameItemBoxPrefab);

                newItem.transform.GetChild(0).GetComponent<Image>().sprite = _dojo.dojoSprite;
                newItem.transform.GetChild(3).transform.GetChild(0).GetComponent<TMP_Text>().text = _dojo.dojoName;
                newItem.transform.GetChild(3).transform.GetChild(1).GetComponent<TMP_Text>().text = _dojo.dojoExplanation;

                if(!VGPGSManager.Instance._playerData.unlockedDojos[current])
                {
                    newItem.transform.GetChild(2).gameObject.SetActive(true);
                    newItem.transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = _dojosHolder.dojos[i].dojoLevel.ToString() + "\nLevel";
                }
                else if(!VGPGSManager.Instance._playerData.boughtBlades[current])
                {
                    newItem.transform.GetChild(2).gameObject.SetActive(false);
                    newItem.transform.GetChild(3).transform.GetChild(2).gameObject.SetActive(true);
                    newItem.transform.GetChild(3).transform.GetChild(2).transform.GetChild(1).GetComponent<TMP_Text>().text = _dojo.dojoPrice.ToString();
                }

                newItem.transform.SetParent(_dojosContainer);
                newItem.transform.localScale = Vector3.one;
                newItem.GetComponent<Button>().onClick.AddListener(() => ButtonManager.Instance.SetPreGameItemDojo(newItem.transform.GetChild(3).transform.GetChild(2).gameObject, newItem.transform, current));
                dojosObjList.Add(newItem);
            }
        }
        public void CheckUnlockedBlades()
        {
            for (int i = 0; i < _bladesHolder.blades.Length; i++)
            {
                if(_bladesHolder.blades[i].bladeLevel <= VGPGSManager.Instance._playerData.level && !VGPGSManager.Instance._playerData.unlockedBlades[i])
                {
                    VGPGSManager.Instance._playerData.unlockedBlades[i] = true;
                    VGPGSManager.Instance._playerData.areNewBladesUnlocked = true;
                }
            }
        }
        public void CheckUnlockedDojos()
        {
            for (int i = 0; i < _dojosHolder.dojos.Length; i++)
            {
                if(_dojosHolder.dojos[i].dojoLevel <= VGPGSManager.Instance._playerData.level && !VGPGSManager.Instance._playerData.unlockedDojos[i])
                {
                    VGPGSManager.Instance._playerData.unlockedDojos[i] = true;
                    VGPGSManager.Instance._playerData.areNewDojosUnlocked = true;
                }
            }
        }
    }
}
