using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Runtime
{
    public class SoundMixerManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private Sprite _soundFXOn, _soundFXOff;
        [SerializeField] private Sprite _musicOn, _musicOff;

        public static SoundMixerManager Instance;
        private void Awake() {
            if(Instance == null) Instance = this;
        }
        private void Start() {
            SetStartVolumes();
        }
        public void SetSoundFXVolume(Image image) => VGPGSManager.Instance._playerData.soundFXVolume = SetVolumes("SoundFXVolume", image, _soundFXOn, _soundFXOff);
        public void SetMusicVolume(Image image) => VGPGSManager.Instance._playerData.musicVolume = SetVolumes("MusicVolume", image, _musicOn, _musicOff);
        private float SetVolumes(string name)
        {
            float outVol = 0f;
            _audioMixer.GetFloat(name, out outVol);

            float targetVol = outVol > -1f ? -80f : 0f;
            _audioMixer.SetFloat(name, targetVol);

            return targetVol;
        }
        private float SetVolumes(string name, Image image = null, Sprite spriteOn = null, Sprite spriteOff = null)
        {
            float outVol = 0f;
            _audioMixer.GetFloat(name, out outVol);

            float targetVol = 0f;
            if(outVol > -1f)
            {
                targetVol = -80f;
                if(image != null && spriteOff != null)
                {
                    image.sprite = spriteOff;
                }
            }
            else
            {
                targetVol = 0f;
                if(image != null && spriteOn != null)
                {
                    image.sprite = spriteOn;
                }
            }
            _audioMixer.SetFloat(name, targetVol);

            return targetVol;
        }
        public void SetStartImages(Image soundFXImage, Image musicImage)
        {
            var startSFXVol = VGPGSManager.Instance._playerData.soundFXVolume;
            var startMusicVol = VGPGSManager.Instance._playerData.musicVolume;

            SetSpritesForSoundImages("SoundFXVolume", startSFXVol, soundFXImage, _soundFXOn, _soundFXOff);
            SetSpritesForSoundImages("MusicVolume", startMusicVol, musicImage, _musicOn, _musicOff);
        }
        private void SetSpritesForSoundImages(string name, float vol, Image image = null, Sprite spriteOn = null, Sprite spriteOff = null)
        {
            _audioMixer.SetFloat(name, vol);
            if(vol > -1f)
            {
                if(image != null && spriteOn != null)
                {
                    image.sprite = spriteOn;
                }
            }
            else
            {
                if(image != null && spriteOff != null)
                {
                    image.sprite = spriteOff;
                }
            }
        }
        private void SetStartVolumes()
        {
            var startSFXVol = VGPGSManager.Instance._playerData.soundFXVolume;
            var startMusicVol = VGPGSManager.Instance._playerData.musicVolume;

            _audioMixer.SetFloat("SoundFXVolume", startSFXVol);
            _audioMixer.SetFloat("MusicVolume", startMusicVol);
        }
    }
}
