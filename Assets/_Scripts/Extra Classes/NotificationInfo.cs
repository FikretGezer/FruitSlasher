using UnityEngine;

namespace Runtime
{
    public class NotificationInfo{
        public Sprite sprite;
        public string desc;
        public int points;

        public NotificationInfo(Sprite notSprite, string notDesc, int notPoints)
        {
            sprite = notSprite;
            desc = notDesc;
            points = notPoints;
        }
    }
}
