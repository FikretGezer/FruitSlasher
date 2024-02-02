using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource sfxObject;
        [SerializeField] private AudioSource musicObject;
        [SerializeField] private AudioClip _clip;
        [field: SerializeField] public AudioClips Clips { get; private set; }
        public List<AudioSource> bombSources = new List<AudioSource>();
        public static SoundManager Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
        }
        private void Update() {
            if(Input.GetKeyDown(KeyCode.H))
                PlaySFXClip(_clip);
        }
        public void PlaySFXClip(AudioClip audioClip)
        {
            // Spawn in gameObject
            AudioSource audioSource = SoundFXPool.Instance.GetSoundFXFromThePool();

            // Assign the audioClip
            audioSource.clip = audioClip;

            // // Assign volume
            // audioSource.volume = volume;

            // Play sound
            audioSource.Play();

            // Get length of SFX clip
            float clipLength = audioSource.clip.length;

            // Add bombSource to the list
            if(audioClip == Clips.bombPop) bombSources.Add(audioSource);

            // Disable the clip after it is done playing
            StartCoroutine(DelayAndDisable(audioSource, clipLength));
        }
        public void PlayRandomSFXClip(AudioClip[] audioClips)
        {
            // Assing random index
            int rand = Random.Range(0, audioClips.Length);

            // Spawn in gameObject
            AudioSource audioSource = SoundFXPool.Instance.GetSoundFXFromThePool();

            // Assign the audioClip
            audioSource.clip = audioClips[rand];

            // // Assign volume
            // audioSource.volume = volume;

            // Play sound
            audioSource.Play();

            // Get length of SFX clip
            float clipLength = audioSource.clip.length;

            // Disable the clip after it is done playing
            StartCoroutine(DelayAndDisable(audioSource, clipLength));
        }
        public void PlayMusic(AudioClip audioClip)
        {
            if(musicObject.clip != audioClip)
            {
                // Assign the audioClip
                musicObject.clip = audioClip;

                // Play music
                musicObject.Play();
            }
        }
        public void StopBombSoundFX()
        {
            foreach(var bombSource in bombSources)
            {
                bombSource.Stop();
                DelayAndDisable(bombSource, 0f);
            }
            bombSources.Clear();
        }
        private IEnumerator DelayAndDisable(AudioSource audioSource, float delay)
        {
            yield return new WaitForSeconds(delay);
            audioSource.gameObject.SetActive(false);
        }
    }
}
