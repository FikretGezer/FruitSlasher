using System.Collections;
using System.Collections.Generic;
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
