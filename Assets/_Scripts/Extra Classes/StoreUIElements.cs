using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Runtime
{
    [System.Serializable]
    public class StoreUIElements{
        public GameObject bladesUI;
        public GameObject dojosUI;
        public BladesHolder bladeHolder;
        public DojosHolder dojoHolder;
        [Header("Item Explanation Container")]
        public GameObject container;
        public Image itemImage;
        public TMP_Text itemName;
        public TMP_Text itemExp;
        public Button itemBuyButton;
        public GameObject itemPriceContainer;
        public TMP_Text itemPrice;
        public GameObject itemLockedImage;
        [Header("Buy Buttons")]
        public Sprite buySprite;
        public Sprite boughtSprite;
        public TMP_Text itemBuyText;

    }
}
