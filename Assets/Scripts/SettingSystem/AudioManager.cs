using SaveSystem;

using UnityEngine;
using UnityEngine.Audio;

namespace SettingSystem
{
    public class AudioManager : MonoBehaviour, ISaveable
    {
        [SerializeField] private AudioMixer audioMixer = null;

        #region Events
        public event VolumeChangedDelegate OnMasterVolumeChanged = null;
        public event VolumeChangedDelegate OnMusicVolumeChanged = null;
        public event VolumeChangedDelegate OnSfxVolumeChanged = null;

        public event VolumeChangedDelegate OnMasterToggle = null;
        public event VolumeChangedDelegate OnMusicToggle = null;
        public event VolumeChangedDelegate OnSfxToggle = null;
        #endregion

        #region Audio Parameters
        private const string MASTER_PARAMETER = "Master";
        private const string MUSIC_PARAMETER = "Music";
        private const string SFX_PARAMETER = "SFX";

        private const string MASTER_TOGGLE_PARAMETER = "MasterToggle";
        private const string MUSIC_TOGGLE_PARAMETER = "MusicToggle";
        private const string SFX_TOGGLE_PARAMETER = "SFXToggle";
        #endregion

        #region Volume Variables
        private float _masterVolume = 1.0f;
        private float _musicVolume = 1.0f;
        private float _sfxVolume = 1.0f;

        private float _masterToggleVolume = 1.0f;
        private float _musicToggleVolume = 1.0f;
        private float _sfxToggleVolume = 1.0f;
        #endregion

        #region Audio Volumes
        public float MasterVolume
        {
            get => _masterVolume;
            set
            {
                value = Mathf.Clamp01(value);
                if (_masterVolume != value)
                {
                    _masterVolume = value;
                    audioMixer.SetFloat(MASTER_PARAMETER, Log10Volume(value));
                    MasterToggleVolume = UpdateToggleVolume(MASTER_PARAMETER);
                    OnMasterVolumeChanged?.Invoke(this, _masterVolume);
                }
            }
        }

        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                value = Mathf.Clamp01(value);
                if (_musicVolume != value)
                {
                    _musicVolume = value;
                    audioMixer.SetFloat(MUSIC_PARAMETER, Log10Volume(value));
                    MusicToggleVolume = UpdateToggleVolume(MUSIC_PARAMETER);
                    OnMusicVolumeChanged?.Invoke(this, _musicVolume);
                }
            }
        }

        public float SfxVolume
        {
            get => _sfxVolume;
            set
            {
                value = Mathf.Clamp01(value);
                if (_sfxVolume != value)
                {
                    _sfxVolume = value;
                    audioMixer.SetFloat(SFX_PARAMETER, Log10Volume(value));
                    SfxToggleVolume = UpdateToggleVolume(SFX_PARAMETER);
                    OnSfxVolumeChanged?.Invoke(this, _sfxVolume);
                }
            }
        }
        #endregion

        #region Audio Toggle Volumes
        public float MasterToggleVolume
        {
            get => _masterToggleVolume;
            set
            {
                value = Mathf.Clamp01(value);
                if (_masterToggleVolume != value)
                {
                    _masterToggleVolume = value;
                    audioMixer.SetFloat(MASTER_TOGGLE_PARAMETER, Log10Volume(value));
                    OnMasterToggle?.Invoke(this, _masterToggleVolume);
                }
            }
        }

        public float MusicToggleVolume
        {
            get => _musicToggleVolume;
            set
            {
                value = Mathf.Clamp01(value);
                if (_musicToggleVolume != value)
                {
                    _musicToggleVolume = value;
                    audioMixer.SetFloat(MUSIC_TOGGLE_PARAMETER, Log10Volume(value));
                    OnMusicToggle?.Invoke(this, _musicToggleVolume);
                }
            }
        }

        public float SfxToggleVolume
        {
            get => _sfxToggleVolume;
            set
            {
                value = Mathf.Clamp01(value);
                if (_sfxToggleVolume != value)
                {
                    _sfxToggleVolume = value;
                    audioMixer.SetFloat(SFX_TOGGLE_PARAMETER, Log10Volume(value));
                    OnSfxToggle?.Invoke(this, _sfxToggleVolume);
                }
            }
        }
        #endregion

        #region Volume Toggling
        public void ToggleMasterVolume() { MasterToggleVolume = ToggleVolume(MASTER_TOGGLE_PARAMETER); }
        public void ToggleMusicVolume() { MusicToggleVolume = ToggleVolume(MUSIC_TOGGLE_PARAMETER); }
        public void ToggleSFXVolume() { SfxToggleVolume = ToggleVolume(SFX_TOGGLE_PARAMETER); }
        #endregion

        #region Private Helpers
        private float ToggleVolume(string volume)
        {
            float value;
            if (audioMixer.GetFloat(volume, out value))
            {
                if (value <= -80f)
                    return 1.0f;
                else
                    return 0.0f;
            }
            return 0.0f;
        }

        private float UpdateToggleVolume(string volume)
        {
            float value;
            if (audioMixer.GetFloat(volume, out value))
            {
                if (value <= -80f)
                    return 0.0f;
                else
                    return 1.0f;
            }
            return 1.0f;
        }

        private float Log10Volume(float value) { return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1.0f)) * 20f; }
        #endregion

        #region Memento
        public IMemento GetSnapshot() => new AudioMemento(this);
        public void RestoreSnapshot(IMemento memento)
        {
            if (memento is not AudioMemento audioMemento)
                throw new System.ArgumentException($"Expected {nameof(AudioMemento)}, got {memento.GetType().Name}", nameof(memento));

            MasterVolume = audioMemento.MasterVolume;
            MusicVolume = audioMemento.MusicVolume;
            SfxVolume = audioMemento.SfxVolume;

            MasterToggleVolume = audioMemento.MasterToggleVolume;
            MusicToggleVolume = audioMemento.MusicToggleVolume;
            SfxToggleVolume = audioMemento.SfxToggleVolume;
        }

        private struct AudioMemento : IMemento
        {
            public readonly float MasterVolume;
            public readonly float MusicVolume;
            public readonly float SfxVolume;

            public readonly float MasterToggleVolume;
            public readonly float MusicToggleVolume;
            public readonly float SfxToggleVolume;

            [Newtonsoft.Json.JsonConstructor]
            public AudioMemento(float masterVolume, float musicVolume, float sfxVolume, float masterToggleVolume, float musicToggleVolume, float sfxToggleVolume)
            {
                MasterVolume = masterVolume;
                MusicVolume = musicVolume;
                SfxVolume = sfxVolume;

                MasterToggleVolume = masterToggleVolume;
                MusicToggleVolume = musicToggleVolume;
                SfxToggleVolume = sfxToggleVolume;
            }

            public AudioMemento(AudioManager audioManager)
            {
                MasterVolume = audioManager.MasterVolume;
                MusicVolume = audioManager.MusicVolume;
                SfxVolume = audioManager.SfxVolume;

                MasterToggleVolume = audioManager.MasterToggleVolume;
                MusicToggleVolume = audioManager.MusicToggleVolume;
                SfxToggleVolume = audioManager.SfxToggleVolume;
            }
        }
        #endregion

        public delegate void VolumeChangedDelegate(AudioManager sender, float value);
    }
}
