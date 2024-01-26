using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class SoundFXPool : MonoBehaviour
    {
        [SerializeField] private AudioSource sfxObjectPrefab;
        private List<AudioSource> _soundFXPool = new List<AudioSource>();

        public static SoundFXPool Instance;
        private void Awake()
        {
            if(Instance == null) Instance = this;
            CreateThePool();
        }

        private void CreateThePool()
        {
            for (int i = 0; i < 15; i++)
            {
                var _source = Instantiate(sfxObjectPrefab);
                _source.gameObject.SetActive(false);
                _source.transform.SetParent(transform);
                _soundFXPool.Add(_source);
            }
        }
        public AudioSource GetSoundFXFromThePool()
        {
            foreach(var sfxSource in _soundFXPool)
            {
                if(!sfxSource.gameObject.activeInHierarchy)
                {
                    sfxSource.gameObject.SetActive(true);
                    return sfxSource;
                }
            }
            return CreateNewSFXSource();
        }
        private AudioSource CreateNewSFXSource()
        {
            var _source = Instantiate(sfxObjectPrefab);
            _source.transform.SetParent(transform);
            _soundFXPool.Add(_source);
            return _source;
        }
    }
}
