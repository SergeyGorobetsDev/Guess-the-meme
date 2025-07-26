using Assets.Code.Scripts.Runtime.State_Machine;
using Assets.Project.Code.Infrastructure.States.App;
using Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem.Windows
{
    public sealed class LooseGameWindow : Window
    {
        [SerializeField]
        private Button restartButton;
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
            restartButton.onClick.AddListener(() =>
            {
                audioPlayer.Play("button-click", MixerTarget.UI);
                stateMachine.SetState<GameplayRestartState>();
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
            restartButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
        }
    }
}