using UnityEngine;

namespace Runtime
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _fruitSource;
        [SerializeField] private AudioSource _knifeSource;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _bombSource;

        [SerializeField] private AudioClips _audioClips;

        public static SoundManager Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
        }
        public void PlayMusicBeginningSFX()
        {
            Invoke("PlayMusic", _audioClips.vihuSFX.length);
            _musicSource.clip = _audioClips.vihuSFX;
            _musicSource.Play();
        }
        private void PlayMusic()
        {
            _musicSource.loop = true;
            _musicSource.clip = _audioClips.roundMusic;
            _musicSource.Play();
        }
        public void PlayFruitPop()
        {
            _fruitSource.PlayOneShot(_audioClips.fruitPopSFX);
        }
        public void PlayKnifeSlicing()
        {
            _knifeSource.PlayOneShot(_audioClips.knifeSliceSFX);
        }
        public void PlayBombPop()
        {
            _bombSource.PlayOneShot(_audioClips.bombPop);
        }
        public void StopBombPop()
        {
            _bombSource.Stop();
        }
        public void PlayBombExplode()
        {
            _musicSource.Stop();
            _bombSource.PlayOneShot(_audioClips.bombExplodeSFX);
            Invoke("PlayMusicAgain", _audioClips.bombExplodeSFX.length);
        }
        private void PlayMusicAgain()
        {
            _musicSource.clip = _audioClips.chillMusic;
            _musicSource.Play();
        }
        public void PlayBombCut()
        {
            _knifeSource.PlayOneShot(_audioClips.bombCuttingSFX);
        }
    }
    [System.Serializable]
    public class AudioClips{
        [Header("SFX")]
        public AudioClip knifeSliceSFX;
        public AudioClip fruitPopSFX;
        public AudioClip bombPop;
        public AudioClip bombExplodeSFX;
        public AudioClip bombCuttingSFX;
        public AudioClip vihuSFX;
        [Header("Musics")]
        public AudioClip roundMusic;
        public AudioClip chillMusic;
    }
}
