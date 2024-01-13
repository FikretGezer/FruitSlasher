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
            AddBlades();
            AddDojos();
            CheckUnlockedBlades();
            CheckUnlockedDojos();
        }
        private void AddBlades()
        {
            for (int i = 0; i < _bladesHolder.blades.Length; i++)
            {
                int current = i;
                var newItem = Instantiate(_preGameItemBoxPrefab);
                newItem.transform.GetChild(0).GetComponent<Image>().sprite = _bladesHolder.blades[i].bladeSprite;
                if(!VGPGSManager.Instance._playerData.unlockedBlades[current])
                {
                    newItem.transform.GetChild(2).gameObject.SetActive(true);
                    newItem.transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = _bladesHolder.blades[i].bladeLevel.ToString() + "\nLevel";
                }

                newItem.transform.SetParent(_bladesContainer);
                newItem.transform.localScale = Vector3.one;
                newItem.GetComponent<Button>().onClick.AddListener(() => ButtonManager.Instance.SetPreGameItemBlade(newItem.transform, current));
                bladesObjList.Add(newItem);
            }
        }
        private void AddDojos()
        {
            for (int i = 0; i < _dojosHolder.dojos.Length; i++)
            {
                int current = i;
                var newItem = Instantiate(_preGameItemBoxPrefab);
                newItem.transform.GetChild(0).GetComponent<Image>().sprite = _dojosHolder.dojos[i].dojoSprite;

                if(!VGPGSManager.Instance._playerData.unlockedDojos[current])
                {
                    newItem.transform.GetChild(2).gameObject.SetActive(true);
                    newItem.transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = _dojosHolder.dojos[i].dojoLevel.ToString() + "\nLevel";
                }

                newItem.transform.SetParent(_dojosContainer);
                newItem.transform.localScale = Vector3.one;
                newItem.GetComponent<Button>().onClick.AddListener(() => ButtonManager.Instance.SetPreGameItemDojo(newItem.transform, current));
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

                    bladesObjList[i].transform.GetChild(2).gameObject.SetActive(false);

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

                    dojosObjList[i].transform.GetChild(2).gameObject.SetActive(false);

                    VGPGSManager.Instance._playerData.areNewDojosUnlocked = true;
                }
            }
        }
    }
}
