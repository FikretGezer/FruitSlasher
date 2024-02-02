using UnityEngine;

namespace Runtime
{
    public class GeneralAnimationEvents : MonoBehaviour
    {
        public void SendItsFinished()
        {
            NotificationController.Instance.IsNotificationShowed = false;
        }
    }
}
