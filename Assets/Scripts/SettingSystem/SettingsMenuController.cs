using Core.InstanceSystem;

using SettingSystem;

using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsMenuController : MenuControllerBase
    {
        [SerializeField] private Button masterSoundButton = null;
        [SerializeField] private Slider masterVolumeSlider = null;
        [SerializeField] private Button musicSoundButton = null;
        [SerializeField] private Slider musicVolumeSlider = null;
        [SerializeField] private Button sfxSoundButton = null;
        [SerializeField] private Slider sfxVolumeSlider = null;

        private AudioManager AudioManager = null;

        private void OnEnable()
        {
            AudioManager = Instanced<AudioManager>.Instance;

            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        #region Menu Control
        protected override void MenuOpened()
        {
            base.MenuOpened();
            AudioManager = Instanced<AudioManager>.Instance;
            InitializeUI();
        }

        protected override void MenuClosed() { }
        #endregion

        #region UI Initialization
        private void InitializeUI()
        {
            masterVolumeSlider.value = AudioManager.MasterVolume;
            musicVolumeSlider.value = AudioManager.MusicVolume;
            sfxVolumeSlider.value = AudioManager.SfxVolume;

            UpdateSoundIconsByVolume();
            UpdateSoundIconsByToggleVolume();
        }
        #endregion

        #region Event Subscriptions
        private void SubscribeToEvents()
        {
            AudioManager.OnMasterVolumeChanged += MasterVolumeChanged;
            AudioManager.OnMusicVolumeChanged += MusicVolumeChanged;
            AudioManager.OnSfxVolumeChanged += SFXVolumeChanged;

            AudioManager.OnMasterToggle += OnSoundToggle;
            AudioManager.OnMusicToggle += OnSoundToggle;
            AudioManager.OnSfxToggle += OnSoundToggle;

            masterSoundButton.onClick.AddListener(AudioManager.ToggleMasterVolume);
            musicSoundButton.onClick.AddListener(AudioManager.ToggleMusicVolume);
            sfxSoundButton.onClick.AddListener(AudioManager.ToggleSFXVolume);

            masterVolumeSlider.onValueChanged.AddListener(MasterVolumeSliderValueChanged);
            musicVolumeSlider.onValueChanged.AddListener(MusicVolumeSliderValueChanged);
            sfxVolumeSlider.onValueChanged.AddListener(SfxVolumeSliderValueChanged);
        }

        private void UnsubscribeFromEvents()
        {
            AudioManager.OnMasterVolumeChanged -= MasterVolumeChanged;
            AudioManager.OnMusicVolumeChanged -= MusicVolumeChanged;
            AudioManager.OnSfxVolumeChanged -= SFXVolumeChanged;

            AudioManager.OnMasterToggle -= OnSoundToggle;
            AudioManager.OnMusicToggle -= OnSoundToggle;
            AudioManager.OnSfxToggle -= OnSoundToggle;

            masterSoundButton.onClick.RemoveListener(AudioManager.ToggleMasterVolume);
            musicSoundButton.onClick.RemoveListener(AudioManager.ToggleMusicVolume);
            sfxSoundButton.onClick.RemoveListener(AudioManager.ToggleSFXVolume);

            masterVolumeSlider.onValueChanged.RemoveListener(MasterVolumeSliderValueChanged);
            musicVolumeSlider.onValueChanged.RemoveListener(MusicVolumeSliderValueChanged);
            sfxVolumeSlider.onValueChanged.RemoveListener(SfxVolumeSliderValueChanged);
        }
        #endregion

        #region Event Handlers
        private void OnSoundToggle(AudioManager sender, float value) { UpdateSoundIconsByToggleVolume(); }

        private void MasterVolumeChanged(AudioManager sender, float value) { masterVolumeSlider.value = value; }
        private void MusicVolumeChanged(AudioManager sender, float value) { musicVolumeSlider.value = value; }
        private void SFXVolumeChanged(AudioManager sender, float value) { sfxVolumeSlider.value = value; }
        #endregion

        #region UI Updates
        private void UpdateSoundIconsByVolume()
        {
            UpdateSoundIcons(masterSoundButton, AudioManager.MasterVolume);
            UpdateSoundIcons(musicSoundButton, AudioManager.MusicVolume);
            UpdateSoundIcons(sfxSoundButton, AudioManager.SfxVolume);
        }

        private void UpdateSoundIconsByToggleVolume()
        {
            UpdateSoundIcons(masterSoundButton, AudioManager.MasterToggleVolume);
            UpdateSoundIcons(musicSoundButton, AudioManager.MusicToggleVolume);
            UpdateSoundIcons(sfxSoundButton, AudioManager.SfxToggleVolume);
        }

        private void UpdateSoundIcons(Button button, float volume)
        {
            bool isNotActive = volume == 0;
            button.transform.GetChild(0).gameObject.SetActive(!isNotActive);
            button.transform.GetChild(1).gameObject.SetActive(isNotActive);
        }
        #endregion

        #region Volume Slider Handlers
        private void MasterVolumeSliderValueChanged(float value) => AudioManager.MasterVolume = value;
        private void MusicVolumeSliderValueChanged(float value) => AudioManager.MusicVolume = value;
        private void SfxVolumeSliderValueChanged(float value) => AudioManager.SfxVolume = value;
        #endregion
    }
}
