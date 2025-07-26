using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Infrastructure.States.App;
using Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem.Windows
{
    public sealed class PauseMenu : Window
    {
        [SerializeField]
        private Button continueButton;
        [SerializeField]
        private Button settingsButton;
        [SerializeField]
        private Button exitButton;

        private StateMachine stateMachine;
        private IAudioPlayer audioPlayer;

        [Inject]
        public void Constructor(StateMachine stateMachine, IWindowsNavigator windowsNavigator, IAudioPlayer audioPlayer)
        {
            this.stateMachine = stateMachine;
            this.windowsNavigator = windowsNavigator;
            this.audioPlayer = audioPlayer;
        }

        protected override void RegisterCallbacks()
        {
            base.RegisterCallbacks();
            continueButton.onClick.AddListener(() =>
            {
                audioPlayer.Play("button-click", MixerTarget.UI);
                windowsNavigator.Pop();
            });

            settingsButton.onClick.AddListener(() =>
            {
                audioPlayer.Play("button-click", MixerTarget.UI);
                windowsNavigator.Show<SettingsWindow>();
            });

            exitButton.onClick.AddListener(() =>
            {
                audioPlayer.Play("button-click", MixerTarget.UI);
                stateMachine.SetState<GameplayEndState>();
            });
        }

        protected override void UnRegisterCallbacks()
        {
            base.UnRegisterCallbacks();
            continueButton.onClick.RemoveAllListeners();
            settingsButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
        }
    }
}