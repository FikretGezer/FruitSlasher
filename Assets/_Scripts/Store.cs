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
        [SerializeField] private Transform _dojosContainer;

        private void Start() {
            AddBlades();
            AddDojos();
        }
        private void AddBlades()
        {
            for (int i = 0; i < _bladesHolder.blades.Length; i++)
            {
                int current = i;
                var newItem = Instantiate(_itemBoxPrefab);
                newItem.transform.GetChild(0).GetComponent<Image>().sprite = _bladesHolder.blades[i].bladeSprite;
                newItem.transform.SetParent(_bladesContainer);
                newItem.transform.localScale = Vector3.one;
                newItem.GetComponent<Button>().onClick.AddListener(() => ButtonManager.Instance.SetItemBlade(current));
            }
        }
        private void AddDojos()
        {
            for (int i = 0; i < _dojosHolder.dojos.Length; i++)
            {
                int current = i;
                var newItem = Instantiate(_itemBoxPrefab);
                newItem.transform.GetChild(0).GetComponent<Image>().sprite = _dojosHolder.dojos[i].dojoSprite;
                newItem.transform.SetParent(_dojosContainer);
                newItem.transform.localScale = Vector3.one;
                newItem.GetComponent<Button>().onClick.AddListener(() => ButtonManager.Instance.SetItemDojo(current));
            }
        }
    }
}
