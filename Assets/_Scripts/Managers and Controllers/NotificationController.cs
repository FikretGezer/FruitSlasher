using System.Collections.Generic;
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

        private Queue<NotificationInfo> missionQ = new Queue<NotificationInfo>();
        public bool IsNotificationShowed { get; set; }
        public static NotificationController Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
        }
        private void Update() {
            if(missionQ.Count > 0 && !IsNotificationShowed)
            {
                IsNotificationShowed = true;
                NotificationInfo mInfo = missionQ.Dequeue();
                SetNotificationUI(mInfo);
            }
        }
        public void EnqueueNotification(Sprite notSprite, string notDesc, int notPoints)
        {
            missionQ.Enqueue(new NotificationInfo(notSprite, notDesc, notPoints));
        }
        private void SetNotificationUI(NotificationInfo info)
        {
            notificationImage.sprite = info.sprite;
            notificationDescription.text = info.desc;
            notificationPoints.text = info.points.ToString();

            notificationAnim.SetTrigger("showNotify");
            SoundManager.Instance.PlaySFXClip(SoundManager.Instance.Clips.notificationSFX);
        }
    }
}
