using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class Store : MonoBehaviour
    {
        [SerializeField] private BladesHolder _bladesHolder;
        [SerializeField] private DojosHolder _dojosHolder;
        [SerializeField] private GameObject _itemBoxPrefab;
        [SerializeField] private Transform _bladesContainer;
        [SerializeField] private Transform _dojosContainer;//
        [SerializeField] private bool isOnMenu;

        private List<GameObject> bladesObjList = new List<GameObject>();
        private List<GameObject> dojosObjList = new List<GameObject>();

        public static Store Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
        }

        private void Start() {
            if(isOnMenu)
            {
                AddBlades();
                AddDojos();
                // CheckUnlockedBlades();
                // CheckUnlockedDojos();
            }
        }
        private void OnEnable() {
            // if(!isOnMenu)
            // {
            // }
            CheckUnlockedBlades();
            CheckUnlockedDojos();
        }
        private void AddBlades()
        {
            for (int i = 0; i < _bladesHolder.blades.Length; i++)
            {
                int current = i;
                var newItem = Instantiate(_itemBoxPrefab);
                newItem.transform.GetChild(0).GetComponent<Image>().sprite = _bladesHolder.blades[i].bladeSprite;
                if(!VGPGSManager.Instance._playerData.unlockedBlades[current])
                {
                    newItem.transform.GetChild(2).gameObject.SetActive(true);
                    newItem.transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = _bladesHolder.blades[i].bladeLevel.ToString() + "\nLevel";
                }

                newItem.transform.SetParent(_bladesContainer);
                newItem.transform.localScale = Vector3.one;
                newItem.GetComponent<Button>().onClick.AddListener(() => ButtonManager.Instance.SetItemBlade(current));
                bladesObjList.Add(newItem);
            }
        }
        private void AddDojos()
        {
            for (int i = 0; i < _dojosHolder.dojos.Length; i++)
            {
                int current = i;
                var newItem = Instantiate(_itemBoxPrefab);
                newItem.transform.GetChild(0).GetComponent<Image>().sprite = _dojosHolder.dojos[i].dojoSprite;

                if(!VGPGSManager.Instance._playerData.unlockedDojos[current])
                {
                    newItem.transform.GetChild(2).gameObject.SetActive(true);
                    newItem.transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = _dojosHolder.dojos[i].dojoLevel.ToString() + "\nLevel";
                }

                newItem.transform.SetParent(_dojosContainer);
                newItem.transform.localScale = Vector3.one;
                newItem.GetComponent<Button>().onClick.AddListener(() => ButtonManager.Instance.SetItemDojo(current));
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

                    if(isOnMenu)
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

                    if(isOnMenu)
                        dojosObjList[i].transform.GetChild(2).gameObject.SetActive(false);

                    VGPGSManager.Instance._playerData.areNewDojosUnlocked = true;
                }
            }
        }
        public void ResetPositionOfScroll()
        {
            void ResetX(Transform t) {
                var pos = t.position;
                pos.x = 0f;
                t.position = pos;
            }
            ResetX(_dojosContainer.transform);
            ResetX(_bladesContainer.transform);
        }
    }
}
