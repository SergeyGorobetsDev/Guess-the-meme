using Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem.Windows
{
    public sealed class SettingsWindow : Window
    {
        [SerializeField]
        private Slider musicVolumeSlider;
        [SerializeField]
        private Slider uiVolumeSlider;
        [SerializeField]
        private Slider sfxVolumeSlider;

        private IAudioPlayer audioPlayer;

        [Inject]
        public void Initialize(IWindowsNavigator windowsNavigator, IAudioPlayer audioPlayer)
        {
            this.windowsNavigator = windowsNavigator;
            this.audioPlayer = audioPlayer;
        }


        protected override void BindDocumentData()
        {
            base.BindDocumentData();
            musicVolumeSlider.value = audioPlayer.GetVolumeMusic();
            uiVolumeSlider.value = audioPlayer.GetVolumeUI();
            sfxVolumeSlider.value = audioPlayer.GetVolumeSFX();
        }

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            musicVolumeSlider.onValueChanged.AddListener(value =>
                audioPlayer.SetVolumeMusic(value));
            uiVolumeSlider.onValueChanged.AddListener(value =>
                audioPlayer.SetVolumeUI(value));
            sfxVolumeSlider.onValueChanged.AddListener(value =>
                audioPlayer.SetVolumeSFX(value));
        }

        protected override void UnRegisterCallbacks()
        {
            base.UnRegisterCallbacks();

            musicVolumeSlider.onValueChanged.RemoveAllListeners();
            uiVolumeSlider.onValueChanged.RemoveAllListeners();
            sfxVolumeSlider.onValueChanged.RemoveAllListeners();
        }
    }
}