using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class NotificationController : MonoBehaviour
    {
        [SerializeField] private Animator notificationAnim;
        [SerializeField] private Image notificationImage;
        [SerializeField] private TMP_Text notificationDescription;
        [SerializeField] private TMP_Text notificationPoints;

        public static NotificationController Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
        }
        public void showNotify()
        {
            notificationAnim.SetTrigger("showNotify");
        }
        public void SetNotification(Sprite notSprite, string notDesc, int notPoints)
        {
            notificationImage.sprite = notSprite;
            notificationDescription.text = notDesc;
            notificationPoints.text = notPoints.ToString();
            notificationAnim.SetTrigger("showNotify");
        }
    }
}
