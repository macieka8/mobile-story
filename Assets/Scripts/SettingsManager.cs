using UnityEngine;
using UnityEngine.Audio;

namespace Game
{
    public class SettingsManager : MonoBehaviour
    {
        public static readonly string GAME_VOLUME_KEY = "Volume";
        public static readonly string MUSIC_VOLUME_KEY = "MusicVolume";
        public static readonly string SOUNDEFFECTS_VOLUME_KEY = "SoundEffectsVolume";

        [Header("Game Volume")]
        [SerializeField] FloatGameEvent _gameVolumeEvent;
        [Header("Music Volume")]
        [SerializeField] AudioMixer _musicMixer;
        [SerializeField] FloatGameEvent _musicVolumeEvent;
        [Header("Sound Effects Volume")]
        [SerializeField] AudioMixer _soundEffectsMixer;
        [SerializeField] FloatGameEvent _soundEffectsVolumeEvent;

        void Start()
        {
            LoadSettings();
        }

        void OnEnable()
        {
            LoadSettings();
        }

        void LoadSettings()
        {
            AudioListener.volume = PlayerPrefs.GetFloat(GAME_VOLUME_KEY, 0.5f);
            _gameVolumeEvent.RaiseEvent(AudioListener.volume);

            var musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);
            var scaledMusicVolume = Mathf.Log10(musicVolume) * 20f;
            _musicMixer.SetFloat("Volume", scaledMusicVolume);
            _musicVolumeEvent.RaiseEvent(musicVolume);

            var soundEffectsVolume = PlayerPrefs.GetFloat(SOUNDEFFECTS_VOLUME_KEY, 1f);
            var scaledSoundEffectsVolume = Mathf.Log10(soundEffectsVolume) * 20f;
            _soundEffectsMixer.SetFloat("Volume", scaledSoundEffectsVolume);
            _soundEffectsVolumeEvent.RaiseEvent(soundEffectsVolume);
        }

        public void SetGameVolume(float volume)
        {
            AudioListener.volume = volume;
            PlayerPrefs.SetFloat(GAME_VOLUME_KEY, volume);
        }

        public void SetMusicVolume(float volume)
        {
            _musicMixer.SetFloat("Volume", Mathf.Log10(volume) * 20f);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        }

        public void SetSoundEffectsVolume(float volume)
        {
            _soundEffectsMixer.SetFloat("Volume", Mathf.Log10(volume) * 20f);
            PlayerPrefs.SetFloat(SOUNDEFFECTS_VOLUME_KEY, volume);
        }
    }
}
