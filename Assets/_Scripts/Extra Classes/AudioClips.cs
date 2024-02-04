using UnityEngine;

namespace Runtime
{
    [System.Serializable]
    public class AudioClips
    {
        [Header("SFX")]
        public AudioClip knifeSliceSFX;
        public AudioClip fruitPopSFX;
        public AudioClip bombPop;
        public AudioClip bombExplodeSFX;
        public AudioClip bombCuttingSFX;
        public AudioClip vihuSFX;
        public AudioClip buttonClickSFX;
        public AudioClip notificationSFX;
        public AudioClip itemBuySFX;
        public AudioClip swipeSoundsSFX;
        public AudioClip swipeMenusSFX;

        [Header("Musics")]
        public AudioClip roundMusic;
        public AudioClip chillMusic;
    }
}
